using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Abstraction.User;
using Hackathon.BL.Email;
using Hackathon.BL.Validation.User;
using Hackathon.Common;
using Hackathon.Common.Configuration;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ValidationException = Hackathon.Common.Exceptions.ValidationException;

namespace Hackathon.BL.User
{
    public class UserService: IUserService
    {
        /// <see cref="SignUpModelValidator"/>
        private readonly IValidator<SignUpModel> _signUpModelValidator;
        private readonly IValidator<SignInModel> _signInModelValidator;

        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly IFileStorageService _fileStorageService;

        private readonly int _requestLifetimeMinutes;

        private readonly IMapper _mapper;

        public UserService(
            IOptions<AppSettings> appSettings,
            IValidator<SignUpModel> signUpModelValidator,
            IValidator<SignInModel> signInModelValidator,
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _signUpModelValidator = signUpModelValidator;
            _signInModelValidator = signInModelValidator;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _requestLifetimeMinutes = appSettings?.Value?.EmailConfirmationRequestLifetime ?? EmailConfirmationService.EmailConfirmationRequestLifetimeDefault;
        }

        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            await _signUpModelValidator.ValidateAndThrowAsync(signUpModel);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password);
            signUpModel.Password = passwordHash;

            return await _userRepository.CreateAsync(signUpModel);
        }

        public async Task<AuthTokenModel> SignInAsync(SignInModel signInModel)
        {
            await _signInModelValidator.ValidateAndThrowAsync(signInModel);

            var users = await _userRepository.GetAsync(new GetListParameters<UserFilter>
            {
                Filter = new UserFilter
                {
                    Username = signInModel.UserName
                },
                Limit = 1
            });

            if (users.Items.Count == 0)
                throw new EntityNotFoundException("Пользователь не найден");

            var user = users.Items.First();

            var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash);

            if (!verified)
                throw new ValidationException("Неправильное имя пользователя или пароль");

            return GenerateToken(user);
        }

        public async Task<AuthTokenModel> SignInByGoogle(SignInByGoogleModel signInByGoogleModel)
        {
            var userModel = await _userRepository.GetByGoogleIdOrEmailAsync(signInByGoogleModel.Id, signInByGoogleModel.Email);

            if (userModel != null)
            {
                userModel.GoogleAccount = _mapper.Map<GoogleAccountModel>(signInByGoogleModel);
                await _userRepository.UpdateGoogleAccount(userModel.GoogleAccount);
            }
            else
            {
                var userId = await _userRepository.CreateAsync(new SignUpModel
                {
                    Email = signInByGoogleModel.Email,
                    UserName = signInByGoogleModel.FullName,
                    FullName = signInByGoogleModel.FullName,
                    Password = signInByGoogleModel.Id,
                    GoogleAccount = signInByGoogleModel
                });

                userModel = await _userRepository.GetAsync(userId);
            }

            return GenerateToken(userModel);
        }

        public async Task<UserModel> GetAsync(long userId)
        {
            if (!await _userRepository.IsExistAsync(userId))
                throw new EntityNotFoundException("Пользователя с указанным идентификатором не существует");

            var model = await _userRepository.GetAsync(userId);

            return EnrichModel(model);
        }

        public async Task<BaseCollection<UserModel>> GetAsync(GetListParameters<UserFilter> getListParameters)
        {
            var models = await _userRepository.GetAsync(getListParameters);

            if (models.Items is {Count: > 0})
                models.Items = models.Items.Select(EnrichModel).ToArray();

            return models;
        }

        public AuthTokenModel GenerateToken(UserModel user)
        {
            var expires = DateTimeOffset.UtcNow.AddMinutes(_appSettings.AuthOptions.LifeTime);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, ((int) user.Role).ToString())
            };

            if (user.GoogleAccount != null)
                claims.Add(new Claim(AppClaimTypes.GoogleId, user.GoogleAccount.Id));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires.UtcDateTime,
                Issuer = _appSettings.AuthOptions.Issuer,
                Audience = _appSettings.AuthOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.AuthOptions.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthTokenModel
            {
                UserId = user.Id,
                Expires = expires.ToUnixTimeMilliseconds(),
                Token = tokenString,
                Role = user.Role,
                GoogleId = user.GoogleAccount?.Id
            };
        }

        public async Task<Guid> UploadProfileImageAsync(long userId, string filename, Stream stream)
        {
            var existedUser = await GetAsync(userId);

            var uploadResult = await _fileStorageService.UploadAsync(stream, Bucket.Avatars, filename, userId);

            if (existedUser.ProfileImageId is not null) {
                await _fileStorageService.DeleteAsync(existedUser.ProfileImageId.Value);
            }

            await _userRepository.UpdateProfileImageAsync(userId, uploadResult.Id);
            return uploadResult.Id;
        }

        private UserModel EnrichModel(UserModel model)
        {
            if (model.Email.ConfirmationRequest is not null)
            {
                if (model.Email.ConfirmationRequest.IsConfirmed)
                    model.Email.Status = UserEmailStatus.Confirmed;
                else if (DateTime.UtcNow < model.Email.ConfirmationRequest.CreatedAt.AddMinutes(_requestLifetimeMinutes))
                    model.Email.Status = UserEmailStatus.Pending;
                else
                    model.Email.Status = UserEmailStatus.NotConfirmed;
            }

            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Abstraction.FileStorage;
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
        private readonly IValidator<SignUpModel> _signUpModelValidator;
        private readonly IValidator<SignInModel> _signInModelValidator;

        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly IFileStorageService _fileStorageService;

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
        }

        /// <inheritdoc cref="IUserService.CreateAsync(SignUpModel)"/>
        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            await _signUpModelValidator.ValidateAndThrowAsync(signUpModel);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password);
            signUpModel.Password = passwordHash;

            var userModel = await _userRepository.CreateAsync(signUpModel);
            return userModel.Id;
        }

        /// <inheritdoc cref="IUserService.SignInAsync(SignInModel)"/>
        public async Task<AuthTokenModel> SignInAsync(SignInModel signInModel)
        {
            await _signInModelValidator.ValidateAndThrowAsync(signInModel);

            var users = await _userRepository.GetAsync(new GetListModel<UserFilterModel>
            {
                Filter = new UserFilterModel
                {
                    Username = signInModel.UserName
                }
            });

            var user = users.Items.First();

            var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash);

            if (!verified)
                throw new ValidationException("Неправильное имя пользователя или пароль");

            return GenerateToken(user);
        }

        /// <inheritdoc cref="IUserService.SignInByGoogle"/>
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
                userModel = await _userRepository.CreateAsync(new SignUpModel
                {
                    Email = signInByGoogleModel.Email,
                    UserName = signInByGoogleModel.Email,
                    FullName = signInByGoogleModel.FullName,
                    Password = signInByGoogleModel.Id,
                    GoogleAccount = signInByGoogleModel
                });
            }

            return GenerateToken(userModel);
        }

        /// <inheritdoc cref="IUserService.GetAsync(long)"/>
        public async Task<UserModel> GetAsync(long userId)
        {
            if (!await _userRepository.ExistAsync(userId))
                throw new EntityNotFoundException("Пользователя с указанным идентификатором не существует");

            return await _userRepository.GetAsync(userId);
        }

        /// <inheritdoc cref="IUserService.GetAsync(GetListModel{T})"/>
        public async Task<BaseCollectionModel<UserModel>> GetAsync(GetListModel<UserFilterModel> getListModel)
        {
            return await _userRepository.GetAsync(getListModel);
        }

        /// <inheritdoc cref="IUserService.GenerateToken(UserModel)"/>
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

        public async Task<UserModel> UploadProfileImageAsync(long userId, string filename, Stream stream)
        {
            var uploadResult = await _fileStorageService.Upload(stream, Bucket.Avatars, filename, userId);

            //TODO: Удалить предыдущую картинку.
            /*var existedUser = await GetAsync(userId);
            if (existedUser.ProfileImageId is not null) { 
                var uploadResult = await _fileStorageService.Delete(existedUser.ProfileImageId);
            }*/

            return await _userRepository.UpdateProfileImageAsync(userId, uploadResult.Id);
        }

        private static class AppClaimTypes
        {
            public const string GoogleId = nameof(GoogleId);
        }
    }
}
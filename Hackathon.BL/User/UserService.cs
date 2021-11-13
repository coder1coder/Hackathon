using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.User.Validators;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.BL.User
{
    public class UserService: IUserService
    {
        private readonly SignUpModelValidator _signUpModelValidator;
        private readonly SignInModelValidator _signInModelValidator;
        private readonly IUserRepository _userRepository;
        private readonly AuthOptions _authConfig;

        public UserService(
            IOptions<AuthOptions> authOptions,
            SignUpModelValidator signUpModelValidator,
            SignInModelValidator signInModelValidator,
            IUserRepository userRepository
            )
        {
            _authConfig = authOptions.Value;
            _signUpModelValidator = signUpModelValidator;
            _signInModelValidator = signInModelValidator;
            _userRepository = userRepository;
        }

        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            await _signUpModelValidator.ValidateAndThrowAsync(signUpModel);

            var canCreate = await _userRepository.CanCreateAsync(signUpModel);

            if (!canCreate)
                throw new ServiceException("Невозможно создать пользователя");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password);
            signUpModel.Password = passwordHash;

            return await _userRepository.CreateAsync(signUpModel);
        }

        public async Task<AuthTokenModel> SignInAsync(SignInModel signInModel)
        {
            await _signInModelValidator.ValidateAndThrowAsync(signInModel);

            var user = await _userRepository.GetAsync(signInModel.UserName);

            if (user == null)
                throw new ServiceException("Пользователя с такими данными не существует");

            var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash);

            if (!verified)
                throw new ServiceException("Неправильное имя пользователя или пароль");

            return GenerateToken(user);
        }

        public async Task<UserModel> GetAsync(long userId)
        {
            var userModel = await _userRepository.GetAsync(userId);

            if (userModel == null)
                throw new ServiceException($"Пользователя с идентификатором {userId} не существует");

            return userModel;
        }

        private AuthTokenModel GenerateToken(UserModel userModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_authConfig.LifeTime),
                Issuer = _authConfig.Issuer,
                Audience = _authConfig.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthTokenModel
            {
                UserId = userModel.Id,
                Expires = tokenDescriptor.Expires,
                Token = tokenString
            };
        }

        private bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _authConfig.Issuer,
                    ValidAudience = _authConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Secret))
                }, out _);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
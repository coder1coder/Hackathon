﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
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

        public UserService(
            IOptions<AppSettings> appSettings,
            IValidator<SignUpModel> signUpModelValidator,
            IValidator<SignInModel> signInModelValidator,
            IUserRepository userRepository
            )
        {
            _appSettings = appSettings.Value;
            _signUpModelValidator = signUpModelValidator;
            _signInModelValidator = signInModelValidator;
            _userRepository = userRepository;
        }

        /// <inheritdoc cref="IUserService.CreateAsync(SignUpModel)"/>
        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            await _signUpModelValidator.ValidateAndThrowAsync(signUpModel);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password);
            signUpModel.Password = passwordHash;

            return await _userRepository.CreateAsync(signUpModel);
        }

        /// <inheritdoc cref="IUserService.SignInAsync(signInModel)"/>
        public async Task<AuthTokenModel> SignInAsync(SignInModel signInModel)
        {
            await _signInModelValidator.ValidateAndThrowAsync(signInModel);

            var users = await _userRepository.GetAsync(new GetFilterModel<UserFilterModel>
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

            return GenerateToken(user.Id);
        }

        /// <inheritdoc cref="IUserService.GetAsync(long)"/>
        public async Task<UserModel> GetAsync(long userId)
        {
            if (!await _userRepository.ExistAsync(userId))
                throw new EntityNotFoundException("Пользователя с указанным идентификатором не существует");

            return await _userRepository.GetAsync(userId);
        }

        /// <inheritdoc cref="IUserService.GetAsync(GetFilterModel{UserFilterModel})"/>
        public async Task<BaseCollectionModel<UserModel>> GetAsync(GetFilterModel<UserFilterModel> getFilterModel)
        {
            return await _userRepository.GetAsync(getFilterModel);
        }

        /// <inheritdoc cref="IUserService.GenerateToken(long)"/>
        public AuthTokenModel GenerateToken(long userId)
        {
            var expires = DateTimeOffset.UtcNow.AddMinutes(_appSettings.AuthOptions.LifeTime);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, userId.ToString()),
                }),
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
                UserId = userId,
                Expires = expires.ToUnixTimeMilliseconds(),
                Token = tokenString
            };
        }
    }
}
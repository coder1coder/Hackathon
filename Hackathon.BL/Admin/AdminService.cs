using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.BL.Admin
{
    public class AdminService : IAdminService
    {
        private readonly AdminOptions _adminConfig;
        private readonly IUserRepository _userRepository;
        private readonly AuthOptions _authConfig;
        private readonly IValidator<SignInModel> _signInModelValidator;

        public AdminService(IOptions<AdminOptions> adminOptions,
            IOptions<AuthOptions> authOptions,
            IValidator<SignInModel> signInModelValidator,
            IUserRepository userRepository)
        {
            _authConfig = authOptions.Value;
            _adminConfig = adminOptions.Value;
            _userRepository = userRepository;
            _signInModelValidator = signInModelValidator;
        }

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

            //var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash);
            var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, _adminConfig.PasswordHash);

            if (!verified)
                throw new ValidationException("Неправильное имя или пароль");

            return GenerateToken(user.Id);
        }

        public AuthTokenModel GenerateToken(long adminId)
        {
            var expires = DateTimeOffset.UtcNow.AddMinutes(_authConfig.LifeTime);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, adminId.ToString()),
                }),
                Expires = expires.UtcDateTime,
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
                UserId = adminId,
                Expires = expires.ToUnixTimeMilliseconds(),
                Token = tokenString
            };
        }
    }
}
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction
{
    public interface IUserService
    {
        Task<long> CreateAsync(SignUpModel signUpModel);
        Task<AuthTokenModel> SignInAsync(SignInModel signInModel);
        Task<UserModel> GetAsync(long userId);
    }
}
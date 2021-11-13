using System.Threading.Tasks;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(SignUpModel signUpModel);
        Task<UserModel> GetAsync(long userId);
        Task<UserModel> GetAsync(string userName);
        Task<bool> CanCreateAsync(SignUpModel signUpModel);
    }
}
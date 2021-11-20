using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction
{
    public interface IUserRepository
    {
        Task<long> CreateAsync(SignUpModel signUpModel);
        Task<UserModel> GetAsync(long userId);
        Task<BaseCollectionModel<UserModel>> GetAsync(GetFilterModel<UserFilterModel> getFilterModel);
        Task<bool> ExistAsync(long userId);
    }
}

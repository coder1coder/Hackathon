using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction
{
    public interface IAdminService
    {
        Task<AuthTokenModel> SignInAsync(SignInModel signInModel);
        AuthTokenModel GenerateToken(long adminId);
    }
}
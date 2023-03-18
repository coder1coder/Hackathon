using BackendTools.Common.Models;
using Hackathon.Common.Models.User;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

public interface IEmailService
{
    Task<Result> SendAsync(EmailParameters parameters);
}

using BackendTools.Common.Models;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.User;

public interface IEmailService
{
    Task<Result> SendAsync(EmailParameters parameters);
}

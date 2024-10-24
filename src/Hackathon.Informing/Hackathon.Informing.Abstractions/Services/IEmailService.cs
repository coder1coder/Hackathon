using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.Users;

namespace Hackathon.Informing.Abstractions.Services;

/// <summary>
/// Сервис по работе с Email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправить Email сообщение
    /// </summary>
    /// <param name="parameters"></param>
    Task<Result> SendAsync(EmailParameters parameters);
}

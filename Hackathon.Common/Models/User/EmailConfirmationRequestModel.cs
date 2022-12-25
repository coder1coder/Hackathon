using System;

namespace Hackathon.Common.Models.User;

/// <summary>
/// Модель запроса подтверждения Email
/// </summary>
public sealed class EmailConfirmationRequestModel: EmailConfirmationRequestParameters
{
    /// <summary>
    /// Дата создания запроса
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата подтверждения Email
    /// </summary>
    public DateTime? ConfirmationDate { get; set; }
}

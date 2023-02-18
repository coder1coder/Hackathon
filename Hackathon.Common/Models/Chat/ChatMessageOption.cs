using System;

namespace Hackathon.Common.Models.Chat;

/// <summary>
/// Опции сообщения
/// </summary>
[Flags]
public enum ChatMessageOption: byte
{
    /// <summary>
    /// По умолчанию
    /// </summary>
    Default = 0,

    /// <summary>
    /// С уведомлением
    /// </summary>
    WithNotify = 1
}

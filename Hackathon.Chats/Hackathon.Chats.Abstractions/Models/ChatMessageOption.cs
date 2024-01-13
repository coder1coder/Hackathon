using System;

namespace Hackathon.Chats.Abstractions.Models;

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

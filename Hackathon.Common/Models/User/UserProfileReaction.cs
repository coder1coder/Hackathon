using System;

namespace Hackathon.Common.Models.User;

/// <summary>
/// Реакция на профиль пользователя
/// </summary>
[Flags]
public enum UserProfileReaction
{
    None = 0,
    Like = 1,
    Heart = 2,
    Fire = 4
}

using Hackathon.Common.Models.Block;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackathon.Common.Models.User;

public class UserModel: UserShortModel
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public UserEmailModel Email { get; set; } = new();

    public GoogleAccountModel GoogleAccount { get; set; }

    /// <summary>
    /// Блокирвока пользователя
    /// </summary>
    public BlockingModel Block { get; set; }

    /// <summary>
    /// Проверяет, заблокирован ли пользователь
    /// </summary>
    /// <returns></returns>
    public bool IsBlocking => 
        Block != null && Block.IsBlocking;
}

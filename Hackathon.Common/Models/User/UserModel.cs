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
    /// Блокировки пользователя
    /// </summary>
    public List<BlockModel> Blocks { get; set; }

    /// <summary>
    /// Проверяет, заблокирован ли пользователь на текущий момент времени
    /// </summary>
    /// <param name="currentDateTime">Текущий момент времени</param>
    /// <returns></returns>
    public bool IsBlocking(DateTime currentDateTime)
        => Blocks.Any(x => x.IsLock(currentDateTime));
}

using System;
using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Contracts.Users;
using Hackathon.Common.Models.Users;
using Refit;

namespace Hackathon.Client;

public interface IUserApi
{
    private const string BaseRoute = "/api/User";

    [Post(BaseRoute)]
    Task<BaseCreateResponse> SignUp([Body] SignUpRequest request);

    [Get(BaseRoute + "/{userId})")]
    Task<UserResponse> Get(long userId);

    [Get(BaseRoute + "/{userId}/reactions")]
    Task<UserProfileReaction> GetReactions(long userId);

    [Post(BaseRoute + "/{userId}/reactions/{reaction}")]
    Task AddReaction(long userId, UserProfileReaction reaction);

    [Delete(BaseRoute + "/{userId}/reactions/{reaction}")]
    Task RemoveReaction(long userId, UserProfileReaction reaction);

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="stream">Файл изображения</param>
    [Post(BaseRoute + "/profile/image/upload")]
    [Multipart]
    Task<Guid> UploadProfileImage([AliasAs("file")] StreamPart stream);

    /// <summary>
    /// Обновить пароль пользователя
    /// </summary>
    /// <param name="parameters">Параметры</param>
    [Put(BaseRoute + "/password")]
    Task UpdatePassword([Body] UpdatePasswordModel parameters);
}

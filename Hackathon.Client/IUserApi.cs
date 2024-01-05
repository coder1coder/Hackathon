using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
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
    /// <returns></returns>
    [Post(BaseRoute + "/profile/image/upload")]
    [Multipart]
    Task<Guid> UploadProfileImage([AliasAs("file")] StreamPart stream);
}

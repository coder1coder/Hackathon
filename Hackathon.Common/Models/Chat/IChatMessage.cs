namespace Hackathon.Common.Models.Chat;

public interface IChatMessage: ICreateChatMessage
{
    /// <summary>
    /// Полное имя автора сообщения
    /// </summary>
    string OwnerFullName { get; set; }

    /// <summary>
    /// Полное имя адресата
    /// </summary>
    string UserFullName { get; set; }
}

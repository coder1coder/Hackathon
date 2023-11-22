namespace Hackathon.Common.Models.User
{
    /// <summary>
    /// Конкретная реакция на профиль пользователя
    /// </summary>
    public class UserProfileReactionModel
    {
        /// <summary>
        /// Количество поставленных реакций
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Тип реакции
        /// </summary>
        public UserProfileReaction Type { get; set; }
    }
}

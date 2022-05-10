namespace Hackathon.Entities
{
    /// <summary>
    /// Таблица для связи участников и команд (отношение многие ко многим)
    /// </summary>
    public class MemberTeamEntity
    {
        /// Команды
        public long TeamId { get; set; }
        public TeamEntity Team { get; set; } = new();

        /// Участники
        public long MemberId { get; set; }
        public UserEntity Member { get; set; } = new();

        /// <summary>
        /// Дата/время добавления участника в команду (UTC)
        /// </summary>
        public DateTime DateTimeAdd { get; set; }
    }
}
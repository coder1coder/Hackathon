using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities
{
    /// <summary>
    /// Команда
    /// </summary>
    public class TeamEntity : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// События
        /// </summary>
        public List<TeamEventEntity> TeamEvents { get; set; } = new ();
        
        /// <summary>
        /// Участники
        /// </summary>
        public List<UserEntity> Users { get; set; } = new ();
        
        /// <summary>
        /// Владелец команды
        /// </summary>
        public UserEntity? Owner { get; set; }
        
        /// <summary>
        /// Идентификатор владельца команды
        /// </summary>
        public long? OwnerId { get; set; }
    }
}
namespace Hackathon.Common.Models.Team
{
    public class DeleteTeamModel
    {
        /// <summary>
        /// Команда которую следует удалить
        /// </summary>
        public long TeamId { get; set; }

        /// <summary>
        /// Событие из которого данную команду удалить
        /// </summary>
        public long? EventId { get; set; }
    }
}

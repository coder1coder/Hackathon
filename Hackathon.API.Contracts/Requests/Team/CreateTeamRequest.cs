using Hackathon.Common.Models.Team;
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests.Team
{
    /// <summary>
    /// Запрос на создание новой команды
    /// </summary>
    public class CreateTeamRequest
    {
        /// <summary>
        /// Наименование команды
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор события
        /// </summary>
        public long? EventId { get; set; }

        /// <summary>
        /// Тип команды
        /// </summary>
        public int Type { get; set; } = (int)TeamType.Private;
    }
}

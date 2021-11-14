using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests.Team
{
    public class CreateTeamRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long EventId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests.Team
{
    public class CreateTeamRequest
    {
        [Required]
        public string Name { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests.Project;

public class ProjectCreateRequest
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public long TeamId { get; set; }

    [Required]
    public long EventId { get; set; }
}
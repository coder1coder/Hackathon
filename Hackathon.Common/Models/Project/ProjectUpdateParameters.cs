using System;
using System.Collections.Generic;

namespace Hackathon.Common.Models.Project;

public class ProjectUpdateParameters: ProjectCreateParameters
{
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор на файлы проекта
    /// </summary>
    public List<Guid> FileIds { get; set; } = new();
}

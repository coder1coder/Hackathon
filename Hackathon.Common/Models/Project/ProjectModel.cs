using Hackathon.Common.Models.FileStorage;

namespace Hackathon.Common.Models.Project
{
    /// <summary>
    /// Проект
    /// </summary>
    public class ProjectModel: ProjectUpdateParameters
    {
        public StorageFile[] Files { get; set; }
    }
}

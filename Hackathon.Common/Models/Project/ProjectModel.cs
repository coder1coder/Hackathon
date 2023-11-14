﻿using Hackathon.Common.Models.Team;
using System;
using Hackathon.FileStorage.Abstraction.Models;

namespace Hackathon.Common.Models.Project;

/// <summary>
/// Проект
/// </summary>
public class ProjectModel: BaseProjectParameters
{
    /// <summary>
    /// Команда
    /// </summary>
    public TeamModel Team { get; set; }

    /// <summary>
    /// Файлы проекта
    /// </summary>
    public StorageFile[] Files { get; set; }

    /// <summary>
    /// Ссылка на репозиторий с указанием ветки
    /// <example>https://github.com/coder1coder/Hackathon/tree/develop</example>
    /// </summary>
    public string LinkToGitBranch { get; set; }

    public Guid[] FileIds { get; set; }
}

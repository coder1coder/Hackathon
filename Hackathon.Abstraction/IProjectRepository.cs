﻿using Hackathon.Common.Models.Project;

namespace Hackathon.Abstraction
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectCreateModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(ProjectCreateModel projectCreateModel);
    }
}
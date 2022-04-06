using Hackathon.Common.Models.ProjectMember;

namespace Hackathon.Abstraction;

public interface IProjectMemberService
{
    
    /// <summary>
    /// Создание участника проекта
    /// </summary>
    /// <param name="createProjectMemberModel">Команда</param>
    /// <returns></returns>
    Task<long> CreateAsync(CreateProjectMemberModel createProjectMemberModel);
    
    /// <summary>
    /// Получение информации об участнике проекта по идентификатору
    /// </summary>
    /// <param name="projectMemberId">Идентификатор роли участника</param>
    /// <returns></returns>
    Task<ProjectMemberModel> GetAsync(long projectMemberId);
    
    
}
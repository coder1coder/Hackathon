using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.ProjectMember;
using Hackathon.Common.Models.ProjectMemberRole;

namespace Hackathon.Abstraction;

public interface IProjectMemberRoleService
{
    /// <summary>
    /// Создание роли участника проекта
    /// </summary>
    /// <param name="createProjectMemberRoleModel"></param>
    /// <returns></returns>
    Task<long> CreateAsync(CreateProjectMemberRoleModel createProjectMemberRoleModel);
    
    /// <summary>
    /// Добавление участника к роли
    /// </summary>
    /// <param name="projectMemberModel">Команда, пользователь</param>
    /// <returns></returns>
    Task AddMemberAsync(ProjectMemberModel projectMemberModel);
    
    /// <summary>
    /// Получить информацию о роли участника проекта
    /// </summary>
    /// <param name="projectMemberRole">Идентификатор роли участника проекта</param>
    /// <returns></returns>
    Task <ProjectMemberRoleModel> GetAsync(long projectMemberRole);
    
    /// <summary>
    /// Получить информацию о ролях участников проектов
    /// </summary>
    /// <param name="getListModel">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<BaseCollectionModel<ProjectMemberRoleModel>> GetAsync(GetListModel<ProjectMemberRoleFilterModel> getListModel);

}
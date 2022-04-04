namespace Hackathon.Abstraction;

public interface IProjectMemberRoleRepository
{
    /// <summary>
    /// Создание роли участника
    /// </summary>
    /// <param name="createProjectMemberRoleModel">Команда</param>
    /// <returns></returns>
    Task<long> CreateAsync(CreateProjectMemberRoleModel createProjectMemberRoleModel);
    
    /// <summary>
    /// Получение информации о роли участника по идентификатору
    /// </summary>
    /// <param name="ProjectMemberRoleId">Идентификатор роли участника</param>
    /// <returns></returns>
    Task<ProjectMemberRoleModel> GetAsync(long projectMemberRoleId);
    
    /// <summary>
    /// Проверка наличия роли по имени
    /// </summary>
    /// <param name="ProjectMemberRoleName">Имя роли участника</param>
    /// <returns><c> true </c> если роль есть, иначе <c> false </c></returns>
    Task<bool> ExistAsync(string projectMemberRoleName);
    
    /// <summary>
    /// Добавление участника с такой же ролью
    /// </summary>
    /// <param name="ProjectMemberRoleModel">Роль, участник</param>
    /// <returns></returns>
    Task AddMemberAsync(ProjectMemberRoleModel projectMemberRole);

    Task RemoveMemberAsync(ProjectMemberRoleModel projectMemberRole);
}
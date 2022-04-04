namespace Hackathon.Abstraction;

public interface IProjectUserRoleRepository
{
    /// <summary>
    /// Создание роли участника
    /// </summary>
    /// <param name="createProjectUserRoleModel">Команда</param>
    /// <returns></returns>
    Task<long> CreateAsync(CreateProjectUserRoleModel createProjectUserRoleModel);
    
    /// <summary>
    /// Получение информации о роли участника по идентификатору
    /// </summary>
    /// <param name="ProjectUserRoleId">Идентификатор роли участника</param>
    /// <returns></returns>
    Task<ProjectUserRoleModel> GetAsync(long projectUserRoleId);
    
    /// <summary>
    /// Проверка наличия роли по имени
    /// </summary>
    /// <param name="ProjectUserRoleName">Имя роли участника</param>
    /// <returns><c> true </c> если роль есть, иначе <c> false </c></returns>
    Task<bool> ExistAsync(string projectUserRoleName);
    
    /// <summary>
    /// Добавление участника с такой же ролью
    /// </summary>
    /// <param name="ProjectUserRoleModel">Роль, участник</param>
    /// <returns></returns>
    Task AddMemberAsync(ProjectUserRoleModel projectUserRole);

    Task RemoveMemberAsync(ProjectUserRoleModel projectUserRole);
}
export enum UserRole
{
  Default = 0,
  Administrator = 1
}

export class UserRoleTranslator{

  public static GetName = (e:UserRole) : string => UserRole[e];

  public static Translate = (e: UserRole) : string => {

    switch (e) {
      case UserRole.Default: return 'Пользователь'
      case UserRole.Administrator: return 'Администратор'

      default: return UserRole[e];
    }
  }
}

namespace Hackathon.BL.Email;

public static class EmailConfirmationErrorMessages
{
    public const string UserWithIdNotFound = "Пользователь с идентификатором '{0}' не найден";
    public const string UsersEmailIsNotListed = "Email пользователя не указан";
    public const string EmailConfirmationTitle = "Hackathon: подтвердите свой Email";
    public const string EmailAlreadyConfirmed = "Email пользователя уже подтвержден";
    public const string ConfirmationCodeIsWrong = "Код подтверждение указан неверно";
    public const string EmailConfirmationRequestWasNotFound = "Запрос на подтверждение Email пользователя не найден";
}

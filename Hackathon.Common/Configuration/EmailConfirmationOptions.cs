namespace Hackathon.Common.Configuration;

public sealed class EmailSenderSettings
{
    /// <summary>
    /// Сервер
    /// </summary>
    /// <remarks>Не может быть NULL или Empty, при регистрации сервиса упадет ошибка</remarks>
    public string Server { get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// SSL
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Почта отправителя
    /// </summary>
    public string Sender { get; set; }
}

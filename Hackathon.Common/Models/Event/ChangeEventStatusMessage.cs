namespace Hackathon.Common.Models.Event
{
    /// <summary>
    /// Сообщение высылаемое командам при смене статуса ивента
    /// </summary>
    public class ChangeEventStatusMessage
    {
        /// <summary>
        /// Статус ивента
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Сообщение командам при смене статуса
        /// </summary>
        public string Message { get; set; }
    }
}
namespace Hackathon.Common.Models.Event
{
    /// <summary>
    /// Модель события для обновления
    /// </summary>
    public class EventUpdateParameters : EventCreateParameters
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        public long Id { get; set; }
    }
}

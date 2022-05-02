namespace Hackathon.Common.Models.Event
{
    /// <summary>
    /// Модель события для обновления
    /// </summary>
    public class UpdateEventModel: CreateEventModel
    {
        public long Id { get; set; }
    }
}
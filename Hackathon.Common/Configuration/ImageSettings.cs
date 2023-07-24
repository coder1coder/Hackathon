namespace Hackathon.Common.Configuration
{
    /// <summary>
    /// Настройки изображения
    /// </summary>
    public class ImageSettings
    {
        /// <summary>
        /// Минимальный вес изображения(в байтах)
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Максимальный вес изображения(в байтах)
        /// </summary>
        public int MaxLength { get; set; }
    }
}

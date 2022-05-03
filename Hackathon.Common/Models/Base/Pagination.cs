namespace Hackathon.Common.Models.Base
{
    public class Pagination
    {
        private int _offset;

        /// <summary>
        /// Количество первых записей которые необходимо пропустить
        /// </summary>
        public int Offset
        {
            get => _offset;
            set => _offset = value > 0 ? value : 0;
        }
        
        /// <summary>
        /// Количество записей которые необходимо получить
        /// </summary>
        public int Limit { get; set; } = 15;
    }
}
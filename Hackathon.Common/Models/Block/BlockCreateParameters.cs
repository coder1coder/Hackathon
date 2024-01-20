using System;

namespace Hackathon.Common.Models.Block
{
    /// <summary>
    /// Модель для создания блокировки
    /// </summary>
    public class BlockCreateParameters
    {
        /// <summary>
        /// Тип блокировки
        /// </summary>
        public BlockType Type { get; set; }

        /// <summary>
        /// Причина блокировки
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Дата, до которой действует
        /// </summary>
        public DateTime? ActionDate { get; set; }

        /// <summary>
        /// Часы, сколько действует
        /// </summary>
        public int? ActionHours { get; set; }

        /// <summary>
        /// Id пользователя, на которого назначена блокировка
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id администратора, который назначает блокировку
        /// </summary>
        public long AdministratorId { get; set; }
    }
}

using System;

namespace Hackathon.Common.Models.Block
{
    /// <summary>
    /// Модель для создания блокировки
    /// </summary>
    public class BlockingCreateParameters
    {
        /// <summary>
        /// Тип блокировки
        /// </summary>
        public BlockingType Type { get; set; }

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
        /// Id пользователя, который назначил блокировку
        /// </summary>
        public long AssignmentUserId { get; set; }

        /// <summary>
        /// Id пользователя, на которого назначена блокировка
        /// </summary>
        public long TargetUserId { get; set; }
    }
}

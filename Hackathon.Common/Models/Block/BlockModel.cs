using System;

namespace Hackathon.Common.Models.Block
{
    /// <summary>
    /// Блокировка пользователей
    /// </summary>
    public class BlockModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип блокировки
        /// </summary>
        public BlockType Type { get; set; }

        /// <summary>
        /// Снята ли блокировка
        /// </summary>
        public bool IsRemove { get; set; }

        /// <summary>
        /// Причина блокировки
        /// </summary>
        public string Reason {  get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата, до которой действует
        /// </summary>
        public DateTime? ActionDate { get; set; }

        /// <summary>
        /// Часы, сколько действует
        /// </summary>
        public int? ActionHours { get; set; }

        /// <summary>
        /// Id администратора, который назначил блокировку
        /// </summary>
        public long AdministratorId { get; set; }

        /// <summary>
        /// Id пользователя, на которого назначена блокировка
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Определяет, действует ли в текущий момент времени блокировка
        /// </summary>
        /// <param name="currentTime">Текущее время</param>
        /// <returns></returns>
        public bool IsLock(DateTime currentDateTime)
        {
            if (IsRemove)
                return false;
            else
            {
                if (Type == BlockType.Permanent)
                    return true;

                if (ActionDate.HasValue && ActionDate.Value >= currentDateTime)
                    return true;

                if (ActionHours.HasValue && CreatedDate.AddHours(ActionHours.Value) >= currentDateTime)
                    return true;

                return false;
            }
        }
    }
}

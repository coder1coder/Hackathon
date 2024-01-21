using Hackathon.Common.Abstraction;
using System;

namespace Hackathon.Common.Models.Block
{
    /// <summary>
    /// Блокировка пользователей
    /// </summary>
    public class BlockingModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BlockingModel(
            BlockingType type,
            string reason,
            long assignmentUserId,
            long targetUserId,
            DateTime? actionDate = default,
            int? actionHours = default)
        {
            Type = type;
            Reason = reason;
            AssignmentUserId = assignmentUserId;
            TargetUserId = targetUserId;
            ActionDate = Type == BlockingType.Temporary 
                ? actionDate ?? DateTime.UtcNow.AddHours(actionHours ?? throw new Exception())
                : null;
        }

        public BlockingModel()
        {
            
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Тип блокировки
        /// </summary>
        public BlockingType Type { get; private set; }

        /// <summary>
        /// Причина блокировки
        /// </summary>
        public string Reason {  get; private set; }

        /// <summary>
        /// Id пользователя, который назначил блокировку
        /// </summary>
        public long AssignmentUserId { get; private set; }

        /// <summary>
        /// Id пользователя, на которого назначена блокировка
        /// </summary>
        public long TargetUserId { get; private set; }

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Дата, до которой действует
        /// </summary>
        public DateTime? ActionDate { get; private set; }

        /// <summary>
        /// Определяет, действует ли блокировка в текущий момент времени
        /// </summary>
        /// <returns></returns>
        public bool IsBlocking => 
            Type == BlockingType.Permanent || ActionDate > DateTime.UtcNow;
    }
}

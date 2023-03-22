using System;

namespace Hackathon.Common.Abstraction;

public interface IHasModifyAt
{
    /// <summary>
    /// Дата и время изменения сущености
    /// </summary>
    public DateTime? ModifyAt { get; set; }
}

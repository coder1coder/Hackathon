﻿using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Abstraction.Events;

/// <summary>
/// Заявления на согласование
/// </summary>
public interface IApprovalApplicationService
{
    /// <summary>
    /// Получить список заявок на согласование
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры фильтрации и пагинации</param>
    /// <returns></returns>
    Task<Result<Page<ApprovalApplicationModel>>> GetListAsync(long authorizedUserId, GetListParameters<ApprovalApplicationFilter> parameters);
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction.Entities;
using Hackathon.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class NotificationRepository: INotificationRepository
{
    private readonly ApplicationDbContext _dbContext;

    private readonly IMapper _mapper;

    public NotificationRepository(
        IMapper mapper,
        ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="INotificationRepository.Push{T}"/> 
    public async Task<Guid> Push<T>(CreateNotificationModel<T> model) where T : class
    {
        var notification = _mapper.Map<NotificationEntity>(model);
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
        return notification.Id;
    }

    /// <inheritdoc cref="INotificationRepository.GetList"/> 
    public async Task<BaseCollectionModel<NotificationModel>> GetList(GetListModel<NotificationFilterModel> model, long userId)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        var total = query.Count();

        if (model.Filter?.IsRead != null)
            query = query.Where(x => x.IsRead == model.Filter.IsRead.Value);

        if (!string.IsNullOrWhiteSpace(model.SortBy))
        {
            if (string.Equals(model.SortBy, nameof(NotificationEntity.CreatedAt), StringComparison.CurrentCultureIgnoreCase))
            {
                query = model.SortOrder == SortOrder.Asc
                    ? query.OrderBy(x => x.CreatedAt)
                    : query.OrderByDescending(x => x.CreatedAt);
            }
        }

        return new BaseCollectionModel<NotificationModel>
        {
            Items = await query
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .ProjectToType<NotificationModel>()
                .ToListAsync(),
            TotalCount = total
        };
    }

    /// <inheritdoc cref="INotificationRepository.GetUnreadNotificationsCount"/> 
    public async Task<long> GetUnreadNotificationsCount(long userId)
        => await _dbContext.Notifications
            .AsNoTracking()
            .CountAsync(x => x.UserId == userId && x.IsRead == false);

    /// <inheritdoc cref="INotificationRepository.MarkAsRead"/>
    public async Task MarkAsRead(long userId, Guid[] ids = null)
    {
        var query = _dbContext.Notifications.Where(x => x.UserId == userId);
            
        if (ids != null)
            query = query.Where(x=>ids.Contains(x.Id));
                
        await query.ForEachAsync(x => x.IsRead = true);

        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc cref="INotificationRepository.Delete"/>
    public async Task Delete(long userId, Guid[] ids = null)
    {
        var notifications = _dbContext
                .Notifications
                .Where(x=>x.UserId == userId);

        if (ids != null)
            notifications = notifications.Where(x => ids.Contains(x.Id));

        _dbContext.Notifications.RemoveRange(notifications);
        await _dbContext.SaveChangesAsync();
    }
}
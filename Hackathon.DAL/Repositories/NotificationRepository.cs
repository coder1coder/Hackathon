using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Notification;
using Hackathon.DAL.Entities;
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

    public async Task<Guid> Push<T>(CreateNotificationModel<T> model) where T : class
    {
        var notification = _mapper.Map<NotificationEntity>(model);
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
        return notification.Id;
    }

    public async Task<BaseCollection<NotificationModel>> GetList(GetListParameters<NotificationFilterModel> parameters, long userId)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        var total = query.Count();

        if (parameters.Filter?.IsRead != null)
            query = query.Where(x => x.IsRead == parameters.Filter.IsRead.Value);

        if (parameters.Filter?.Group is not null)
            query = query.Where(x => parameters.Filter.Group.Value.GetNotificationTypes().Contains(x.Type));

        if (!string.IsNullOrWhiteSpace(parameters.SortBy)
            && string.Equals(parameters.SortBy, nameof(NotificationEntity.CreatedAt), StringComparison.CurrentCultureIgnoreCase))
        {
            query = parameters.SortOrder == SortOrder.Asc
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt);
        }

        var items = await query
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToArrayAsync();

        return new BaseCollection<NotificationModel>
        {
            Items = _mapper.Map<NotificationEntity[], NotificationModel[]>(items),
            TotalCount = total
        };
    }

    public Task<long> GetUnreadNotificationsCount(long userId)
        => _dbContext.Notifications
            .AsNoTracking()
            .LongCountAsync(x => x.UserId == userId && x.IsRead == false);

    public async Task MarkAsRead(long userId, Guid[] ids)
    {
        var query = _dbContext.Notifications.Where(x => x.UserId == userId);

        if (ids?.Length > 0)
            query = query.Where(x=>ids.Contains(x.Id));

        await query.ForEachAsync(x => x.IsRead = true);

        await _dbContext.SaveChangesAsync();
    }

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

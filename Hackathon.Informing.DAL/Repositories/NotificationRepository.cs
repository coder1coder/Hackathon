using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Informing.Abstractions.Models;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Informing.DAL.Repositories;

public class NotificationRepository: INotificationRepository
{
    private readonly InformingDbContext _dbContext;

    private readonly IMapper _mapper;

    public NotificationRepository(
        IMapper mapper,
        InformingDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Guid[]> AddManyAsync<T>(CreateNotificationModel<T>[] notifications) where T : class
    {
        var notificationEntities = _mapper.Map<NotificationEntity[]>(notifications);
        await _dbContext.Notifications.AddRangeAsync(notificationEntities);
        await _dbContext.SaveChangesAsync();
        return notificationEntities
            .Select(x => x.Id)
            .ToArray();
    }

    public async Task<BaseCollection<NotificationModel>> GetList(GetListParameters<NotificationFilterModel> parameters, long userId)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        var total = query.Count();

        if (parameters.Filter?.IsRead != null)
            query = query.Where(x => x.IsRead == parameters.Filter.IsRead.Value);

        if (!string.IsNullOrWhiteSpace(parameters.SortBy)
            && string.Equals(parameters.SortBy, nameof(NotificationEntity.CreatedAt), StringComparison.CurrentCultureIgnoreCase))
        {
            query = parameters.SortOrder == SortOrder.Asc
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt);
        }

        return new BaseCollection<NotificationModel>
        {
            Items = await query
                .Skip(parameters.Offset)
                .Take(parameters.Limit)
                .ProjectToType<NotificationModel>()
                .ToListAsync(),
            TotalCount = total
        };
    }

    public Task<int> GetUnreadNotificationsCount(long userId)
        => _dbContext.Notifications
            .CountAsync(x => 
                x.UserId == userId 
                && x.IsRead == false);

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

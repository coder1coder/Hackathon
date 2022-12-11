using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Common.Models.FileStorage;
using Hackathon.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hackathon.DAL.Repositories;

public class FileStorageRepository: IFileStorageRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    public FileStorageRepository(ApplicationDbContext dbContext, IMapper mapper, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task AddAsync(StorageFile storageFile)
    {
        var entity = _mapper.Map<FileStorageEntity>(storageFile);
        _dbContext.StorageFiles.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<StorageFile> GetAsync(Guid id)
    {
        if (!_memoryCache.TryGetValue(id, out FileStorageEntity entity))
        {
            entity = await _dbContext.StorageFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _memoryCache.Set(entity.Id, entity,
                    new MemoryCacheEntryOptions
                    {
                        Size = entity.Length,
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                    });
            }
        }

        return _mapper.Map<StorageFile>(entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        var entity =  await _dbContext.StorageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            _dbContext.Remove(entity);
            _memoryCache.Remove(entity.Id);
            await _dbContext.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.FileStorage;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.FileStorage;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class FileStorageRepository: IFileStorageRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public FileStorageRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(StorageFile storageFile)
    {
        var entity = _mapper.Map<FileStorageEntity>(storageFile);
        _dbContext.StorageFiles.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<StorageFile> GetAsync(Guid fileId)
    {
        var entity = await _dbContext.StorageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == fileId);

        return entity is not null
            ? _mapper.Map<StorageFile>(entity)
            : null;
    }

    public async Task<IEnumerable<StorageFile>> GetIsDeletedFilesAsync()
    {
        var files = await _dbContext.StorageFiles
            .AsNoTracking()
            .Where(x => x.IsDeleted == true)
            .ProjectToType<StorageFile>()
            .ToArrayAsync();

        return files;
    }

    public async Task RemoveAsync(Guid fileId)
    {
        var entity = await _dbContext.StorageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == fileId);

        if (entity is not null)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateFlagIsDeleted(Guid fileId, bool flagValue)
    {
        var entity = await _dbContext.StorageFiles
            .FirstOrDefaultAsync(x => x.Id == fileId);

        if (entity is not null)
        {
            entity.IsDeleted = flagValue;
            await _dbContext.SaveChangesAsync();
        }
    }
}

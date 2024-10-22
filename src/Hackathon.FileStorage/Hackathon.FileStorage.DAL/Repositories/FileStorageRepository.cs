using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.DAL.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.FileStorage.DAL.Repositories;

public class FileStorageRepository: IFileStorageRepository
{
    private readonly FileStorageDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public FileStorageRepository(FileStorageDbContext dbContext, IMapper mapper)
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

    public Task<Guid[]> GetIsDeletedFileIdsAsync()
        => _dbContext.StorageFiles
            .Where(x => x.IsDeleted)
            .Select(x => x.Id)
            .ToArrayAsync();

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

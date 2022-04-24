using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.Entities;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Common.Models.FileStorage;
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

    public async Task Add(StorageFile storageFile)
    {
        var entity = _mapper.Map<FileStorageEntity>(storageFile);
        _dbContext.StorageFiles.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<StorageFile> Get(Guid id)
    {
        var entity =  await _dbContext.StorageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<StorageFile>(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity =  await _dbContext.StorageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity != null)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
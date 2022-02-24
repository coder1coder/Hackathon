using Hackathon.Common.Models.FileStorage;

namespace Hackathon.Abstraction.FileStorage;

public interface IFileStorageRepository
{
    Task Add(StorageFile storageFile);
    Task<StorageFile> Get(Guid id);
    Task Remove(Guid id);
}
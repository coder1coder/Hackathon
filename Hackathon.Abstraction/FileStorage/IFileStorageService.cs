using Hackathon.Common.Models.FileStorage;

namespace Hackathon.Abstraction.FileStorage;

public interface IFileStorageService
{
    Task<StorageFile> Upload(Stream stream, Bucket bucket, string fileName, long? ownerId = null);
    Task<Stream> Get(Guid storageFileId);
}
using BackendTools.Common.Models;
using Hackathon.Common.Models.FileStorage;

namespace Hackathon.Abstraction.FileStorage;

public interface IFileStorageService
{
    /// <summary>
    /// Загрузить файл в хранилище
    /// </summary>
    /// <param name="stream">Поток</param>
    /// <param name="bucket">Bucket</param>
    /// <param name="fileName">Имя файла</param>
    /// <param name="ownerId">Идентификатор владельца</param>
    /// <returns></returns>
    Task<StorageFile> UploadAsync(Stream stream, Bucket bucket, string fileName, long? ownerId = null);

    /// <summary>
    /// Получить файл из хранилища
    /// </summary>
    /// <param name="storageFileId">Идентификатор файла</param>
    /// <returns></returns>
    Task<Stream> GetAsync(Guid storageFileId);

    /// <summary>
    /// Удалить файл из хранилища
    /// </summary>
    /// <param name="storageFileId">Идентификатор файла</param>
    /// <returns></returns>
    Task<Result> DeleteAsync(Guid storageFileId);
}

using System;
using System.IO;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.FileStorage.Abstraction.Models;

namespace Hackathon.FileStorage.Abstraction.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Загрузить файл в хранилище
    /// </summary>
    /// <param name="stream">Поток</param>
    /// <param name="bucket">Bucket</param>
    /// <param name="fileName">Имя файла</param>
    /// <param name="ownerId">Идентификатор владельца</param>
    /// <param name="isDeleted">Признак удаленного файла</param>
    Task<StorageFile> UploadAsync(Stream stream, Bucket bucket, string fileName, long? ownerId = null, bool isDeleted = false);

    /// <summary>
    /// Получить файл из хранилища
    /// </summary>
    /// <param name="storageFileId">Идентификатор файла</param>
    Task<Stream> GetAsync(Guid storageFileId);

    /// <summary>
    /// Удалить файл из хранилища
    /// </summary>
    /// <param name="storageFileId">Идентификатор файла</param>
    Task<Result> DeleteAsync(Guid storageFileId);
}

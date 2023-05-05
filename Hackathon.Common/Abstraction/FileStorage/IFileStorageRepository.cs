using Hackathon.Common.Models.FileStorage;
using System;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.FileStorage;

public interface IFileStorageRepository
{
    /// <summary>
    /// Добавить запись о файле
    /// </summary>
    /// <param name="storageFile"></param>
    /// <returns></returns>
    Task AddAsync(StorageFile storageFile);

    /// <summary>
    /// Получить информацию о файле
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <returns></returns>
    Task<StorageFile> GetAsync(Guid fileId);

    /// <summary>
    /// Удалить запись о файле
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <returns></returns>
    Task RemoveAsync(Guid fileId);
}

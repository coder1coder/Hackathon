using Hackathon.Common.Models.FileStorage;
using System;
using System.Collections.Generic;
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

    /// <summary>
    /// Обновить флаг IsDeleted у файла
    /// </summary>
    /// <param name="fileId"></param>
    /// <param name="flagValue"></param>
    /// <returns></returns>
    Task UpdateFlagIsDeleted(Guid fileId, bool flagValue);

    /// <summary>
    /// Получить файлы, которые нужно удалить
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<StorageFile>> GetIsDeletedFilesAsync();
}

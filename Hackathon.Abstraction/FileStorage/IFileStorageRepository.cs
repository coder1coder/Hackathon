using Hackathon.Common.Models.FileStorage;

namespace Hackathon.Abstraction.FileStorage;

public interface IFileStorageRepository
{
    /// <summary>
    /// Добавить запись о файле
    /// </summary>
    /// <param name="storageFile"></param>
    /// <returns></returns>
    Task Add(StorageFile storageFile);
    
    /// <summary>
    /// Получить информацию о файле
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    Task<StorageFile> Get(Guid id);
    
    /// <summary>
    /// Удалить запись о файле
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <returns></returns>
    Task Remove(Guid id);
}
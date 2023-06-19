using System;

namespace Hackathon.DAL.Entities;

public class FileStorageEntity
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя бакета
    /// </summary>
    public string BucketName { get; set; }

    /// <summary>
    /// Имя файла
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Путь к файлу в файловом хранилище
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// MIME тип содержимого
    /// </summary>
    public string MimeType { get; set; }

    /// <summary>
    /// Размер содержимого
    /// </summary>
    public long? Length { get; set; }

    /// <summary>
    /// Идентификатор пользователя загрузившего файл
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Флаг - нужно ли удалять файл
    /// </summary>
    public bool IsDeleted { get; set; }
}

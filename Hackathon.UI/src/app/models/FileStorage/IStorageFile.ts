export interface IStorageFile
{
  /// <summary>
  /// Уникальный идентификатор
  /// </summary>
  id: string;

  /// <summary>
  /// Имя бакета
  /// </summary>
  bucketName: string;

  /// <summary>
  /// Имя файла
  /// </summary>
  fileName: string;

  /// <summary>
  /// Путь к файлу в файловом хранилище
  /// </summary>
  filePath: string;

  /// <summary>
  /// MIME тип содержимого
  /// </summary>
  mimeType: string;

  /// <summary>
  /// Размер содержимого
  /// </summary>
  length?: number;

  /// <summary>
  /// Идентификатор пользователя загрузившего файл
  /// </summary>
  ownerId?: number;
}

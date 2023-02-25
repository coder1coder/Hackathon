using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using BackendTools.Common.Models;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Common;
using Hackathon.Common.Models.FileStorage;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.FileStorage;

public class FileStorageService : IFileStorageService
{
    private const string FileWithIdNotFoundPattern = "Файл с индентификатором {0} не найден";

    private readonly AmazonS3Client _s3Client;
    private readonly ILogger<FileStorageService> _logger;
    private readonly IFileStorageRepository _fileStorageRepository;

    public FileStorageService(AmazonS3Client s3Client, ILogger<FileStorageService> logger, IFileStorageRepository fileStorageRepository)
    {
        _s3Client = s3Client;
        _logger = logger;
        _fileStorageRepository = fileStorageRepository;
    }

    public async Task<StorageFile> UploadAsync(Stream stream, Bucket bucket, string fileName, long? ownerId = null)
    {
        try
        {
            var uniqueFileName = Guid.NewGuid();
            var mimeType = MimeTypeMap.GetMimeType(fileName);
            var bucketName = bucket.ToBucketName();

            var buckets = await _s3Client.ListBucketsAsync();

            if (buckets.Buckets.All(x => x.BucketName != bucketName))
            {
                await _s3Client.PutBucketAsync(bucketName);
            }

            //Размер необходимо зафиксировать до того, как будет вычитан поток
            //который освободится после прочтения
            var streamLength = stream.Length;

            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                Key = uniqueFileName.ToString(),
                BucketName = bucketName,
                InputStream = stream,
                ContentType = mimeType,
                AutoResetStreamPosition = true
            });

            var storageFile = new StorageFile
            {
                Id = uniqueFileName,
                BucketName = bucketName,
                FileName = fileName,
                FilePath = uniqueFileName.ToString(),
                Length = streamLength,
                MimeType = mimeType,
                OwnerId = ownerId
            };

            await _fileStorageRepository.AddAsync(storageFile);

            return storageFile;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Initiator}. Ошибка во время записи файла в файловое хранилище",
                nameof(FileStorageService));
            throw;
        }
    }

    public async Task<Stream> GetAsync(Guid storageFileId)
    {
        var fileInfo = await _fileStorageRepository.GetAsync(storageFileId);

        if (fileInfo is null)
        {
            _logger.LogError("{Initiator}: Файл с индентификатором {storageFileId} не найден",
                nameof(FileStorageService),
                storageFileId);

            throw new FileNotFoundException(string.Format(FileWithIdNotFoundPattern, storageFileId));
        }

        using var storageFile = await _s3Client.GetObjectAsync(
            fileInfo.BucketName,
            storageFileId.ToString());

        await using var responseStream = storageFile.ResponseStream;
        var ms = new MemoryStream();
        await responseStream.CopyToAsync(ms);
        if (ms.CanSeek) ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }

    public async Task<Result> DeleteAsync(Guid storageFileId)
    {
        var fileInfo = await _fileStorageRepository.GetAsync(storageFileId);

        if (fileInfo is null)
        {
            _logger.LogError("{Initiator}: Файл с индентификатором {storageFileId} не найден",
                nameof(FileStorageService),
                storageFileId);

            return Result.NotFound(string.Format(FileWithIdNotFoundPattern, storageFileId));
        }

        await _fileStorageRepository.RemoveAsync(fileInfo.Id);

        var _ = await _s3Client.DeleteObjectAsync(
            fileInfo.BucketName,
            storageFileId.ToString());

        return Result.Success;
    }
}

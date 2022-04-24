﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Common;
using Hackathon.Common.Models.FileStorage;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.FileStorage
{
    public class FileStorageService: IFileStorageService
    {
        private readonly AmazonS3Client _s3Client;
        private readonly ILogger<FileStorageService> _logger;
        private readonly IFileStorageRepository _fileStorageRepository;

        public FileStorageService(AmazonS3Client s3Client, ILogger<FileStorageService> logger, IFileStorageRepository fileStorageRepository)
        {
            _s3Client = s3Client;
            _logger = logger;
            _fileStorageRepository = fileStorageRepository;
        }

        public async Task<StorageFile> Upload(Stream stream, Bucket bucket, string fileName, long? ownerId = null)
        {
            try
            {
                var uniqueFileName = Guid.NewGuid();
                var mimeType = MimeTypeMap.GetMimeType(fileName);
                var bucketName = bucket.ToBucketName();

                var buckets = await _s3Client.ListBucketsAsync();

                if (buckets.Buckets.All(x => x.BucketName != bucketName))
                    await _s3Client.PutBucketAsync(bucketName);

                await _s3Client.PutObjectAsync(new PutObjectRequest()
                {
                    Key = uniqueFileName.ToString(),
                    BucketName = bucketName,
                    InputStream = stream,
                    ContentType = mimeType,
                    AutoResetStreamPosition = true,
                });

                var storageFile = new StorageFile()
                {
                    Id = uniqueFileName,
                    BucketName = bucketName,
                    FileName = fileName,
                    FilePath = uniqueFileName.ToString(),
                    Length = stream.Length,
                    MimeType = mimeType,
                    OwnerId = ownerId
                };

                await _fileStorageRepository.Add(storageFile);

                return storageFile;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка во время записи файла в файловое хранилище");
                throw;
            }
        }

        public async Task<Stream> Get(Guid storageFileId)
        {
            var fileInfo = await _fileStorageRepository.Get(storageFileId);

            using var storageFile = await _s3Client.GetObjectAsync(
                fileInfo.BucketName,
                storageFileId.ToString());

            await using var responseStream = storageFile.ResponseStream;
            var ms = new MemoryStream();
            await responseStream.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }

    }
}
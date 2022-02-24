using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Common.Models.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers;

public class FileStorageController: BaseController
{
    private readonly IFileStorageService _fileStorageService;

    public FileStorageController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost(nameof(Upload))]
    public async Task<StorageFile> Upload([Required] IFormFile file, [Required] Bucket bucket)
    {
        await using var stream = file.OpenReadStream();
        return await _fileStorageService.Upload(stream, bucket, file.FileName);
    }

    [HttpGet]
    [Route("get/{storageFileId:guid}")]
    public async Task<Stream> Get([FromRoute] Guid storageFileId)
    {
        return await _fileStorageService.Get(storageFileId);
    }
}
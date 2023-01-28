using System;
using System.IO;
using System.Threading.Tasks;
using Hackathon.Abstraction.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

[SwaggerTag("Файловое хранилище")]
public class FileStorageController: BaseController
{
    private readonly IFileStorageService _fileStorageService;

    public FileStorageController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload/{bucket:int}")]
    public async Task<IActionResult> Upload([FromRoute] int bucket, IFormFile file)
    {
        if (file is null)
            return BadRequest("Файл не может быть пустым.");

        await using var stream = file.OpenReadStream();
        return Ok(await _fileStorageService.UploadAsync(stream, (Bucket)bucket, file.FileName, UserId));
    }

    [HttpGet]
    [Route("get/{storageFileId:guid}")]
    public Task<Stream> Get([FromRoute] Guid storageFileId)
        => _fileStorageService.GetAsync(storageFileId);

}

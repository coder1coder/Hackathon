using Hackathon.Common.Abstraction.FileStorage;
using System;
using System.IO;
using System.Threading.Tasks;
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
    
    [HttpGet]
    [Route("get/{storageFileId:guid}")]
    public Task<Stream> Get([FromRoute] Guid storageFileId)
        => _fileStorageService.GetAsync(storageFileId);
}

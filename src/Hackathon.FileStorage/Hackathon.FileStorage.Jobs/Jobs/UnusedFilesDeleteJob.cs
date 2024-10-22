using System;
using System.Threading.Tasks;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.Jobs;
using Microsoft.Extensions.Logging;

namespace Hackathon.FileStorage.Jobs.Jobs;

public sealed class UnusedFilesDeleteJob : BaseBackgroundJob<UnusedFilesDeleteJob>
{
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UnusedFilesDeleteJob> _logger;

    public UnusedFilesDeleteJob(
        IFileStorageRepository fileStorageRepository,
        IFileStorageService fileStorageService,
        ILogger<UnusedFilesDeleteJob> logger):base(logger)
    {
        _fileStorageRepository = fileStorageRepository;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public override async Task DoWork()
    {
        var toDeleteFileIds = await _fileStorageRepository.GetIsDeletedFileIdsAsync();

        if (toDeleteFileIds is not { Length: > 0 })
        {
            _logger.LogInformation("{Source} Нет неиспользованных файлов для удаления", nameof(UnusedFilesDeleteJob));
            return;
        }

        var deletedFilesCount = 0;
        foreach (var fileId in toDeleteFileIds)
        {
            try
            {
                var result = await _fileStorageService.DeleteAsync(fileId);
                if (result.IsSuccess)
                {
                    deletedFilesCount++;
                    continue;
                }

                _logger.LogError("{Source} Не удалось удалить файл ID: {FileId}",
                        nameof(UnusedFilesDeleteJob), fileId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Source} Не удалось удалить файл ID: {FileId})",
                    nameof(UnusedFilesDeleteJob),
                    fileId);
            }
        }

        _logger.LogInformation("{Source} Удалено {DeletedFilesCount} из {ToDeleteFilesCount} файлов",
            nameof(UnusedFilesDeleteJob), deletedFilesCount, toDeleteFileIds.Length);
    }
}

using System;
using Hackathon.Common.Abstraction.FileStorage;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Quartz;

namespace Hackathon.Jobs.Events;

public class UnusedFilesDeleteJob : BaseJob<UnusedFilesDeleteJob>
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

    protected override async Task DoWork(IJobExecutionContext context)
    {
        var toDeleteFileIds = await _fileStorageRepository.GetIsDeletedFilesAsync();

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

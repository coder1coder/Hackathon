using Hackathon.Common.Abstraction.FileStorage;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hackathon.Jobs.Events;

public class UnusedFilesDeleteJob : IJob
{
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UnusedFilesDeleteJob> _logger;

    public UnusedFilesDeleteJob(
        IFileStorageRepository fileStorageRepository,
        IFileStorageService fileStorageService,
        ILogger<UnusedFilesDeleteJob> logger)
    {
        _fileStorageRepository = fileStorageRepository;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var filesIds = await _fileStorageRepository.GetIsDeletedFilesAsync();

        var countFiles = 0;
        foreach (var fileId in filesIds)
        {
            var result = await _fileStorageService.DeleteAsync(fileId);
            if (result.IsSuccess)
                countFiles++;
            else
                _logger.LogError("{Source} Не удалось удалить файл с Id: {Id}", 
                    nameof(UnusedFilesDeleteJob), fileId);
        }

        if (countFiles > 0)
            _logger.LogInformation("{Source} Было удалено: {countFiles} неиспользуемых файл/ов/а",
                nameof(UnusedFilesDeleteJob), countFiles);
        else
            _logger.LogInformation("{Source} Нет неиспользованных файлов для удаления",
                nameof(UnusedFilesDeleteJob));
    }
}

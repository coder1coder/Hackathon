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
        var files = await _fileStorageRepository.GetIsDeletedFilesAsync();

        var countFiles = 0;
        foreach (var file in files)
        {
            var result = await _fileStorageService.DeleteAsync(file.Id);
            if (result.IsSuccess)
            {
                await _fileStorageRepository.RemoveAsync(file.Id);
                countFiles++;
            }
            else
            {
                _logger.LogError("Не удалось удалить файл с Id: {Id}",
                    file.Id);
            }  
        }

        if (countFiles > 0)
            _logger.LogInformation("Было удалено: {countFiles} неиспользуемых файл/ов/а",
                countFiles);
        else
            _logger.LogInformation("Нет неиспользуемых файлов для удаления",
                countFiles);
    }
}

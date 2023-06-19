using Hackathon.Common.Abstraction.FileStorage;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hackathon.Jobs.Events;

public class UnusedFilesDeleteJob : IJob
{
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly ILogger<UnusedFilesDeleteJob> _logger;

    public UnusedFilesDeleteJob(
        IFileStorageRepository fileStorageRepository,
        ILogger<UnusedFilesDeleteJob> logger)
    {
        _fileStorageRepository = fileStorageRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var files = await _fileStorageRepository.GetIsDeletedFilesAsync();

        var countFiles = 0;
        foreach (var file in files)
        {
            await _fileStorageRepository.RemoveAsync(file.Id);
            countFiles++;
        }

        _logger.LogInformation("Было удалено: {countFiles} файл/ов/а",
            countFiles);
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts;
using TennisByTheSea.Shared.Contracts;

namespace TennisByTheSea.Application.BackgroundServices.FileProcessing;

public sealed class FileProcessingService : BackgroundService
{
    private readonly IReadOnlyFileProcessingChannel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FileProcessingService> _logger;

    /// <inheritdoc />
    public FileProcessingService(IReadOnlyFileProcessingChannel channel, IServiceProvider serviceProvider, ILogger<FileProcessingService> logger)
    {
        _channel = channel;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (string filename in _channel.ReadAll(stoppingToken).WithCancellation(stoppingToken))
        {
            if (!File.Exists(filename))
            {
                _logger.LogError("File Process failure, file {Filename} not found", filename);
                continue;
            }

            using IServiceScope scope = _serviceProvider.CreateScope();
            IResultProcessor processor = scope.ServiceProvider.GetRequiredService<IResultProcessor>();
            try
            {
                await using FileStream stream = File.OpenRead(filename);
                await processor.ProcessAsync(stream, stoppingToken);
            }
            finally
            {
                File.Delete(filename);
            }
        }
    }
}

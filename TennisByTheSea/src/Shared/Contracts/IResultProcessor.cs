namespace TennisByTheSea.Shared.Contracts;

public interface IResultProcessor
{
    Task ProcessAsync(FileStream stream, CancellationToken stoppingToken);
}

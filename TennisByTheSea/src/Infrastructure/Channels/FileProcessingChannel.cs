using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using TennisByTheSea.Domain.Contracts;

namespace TennisByTheSea.Infrastructure.Channels;

public sealed class FileProcessingChannel : IFileProcessingChannel
{
    private readonly ILogger<FileProcessingChannel> _logger;
    private readonly Channel<string> _channel;
    private const int MaxMessagesInChannel = 100; // move to configuration?

    public FileProcessingChannel(ILogger<FileProcessingChannel> logger)
    {
        _logger = logger;

        BoundedChannelOptions options = new(MaxMessagesInChannel) { SingleWriter = false, SingleReader = true, };
        _channel = Channel.CreateBounded<string>(options);
    }

    public async Task<bool> AddFileAsync(string filename, CancellationToken cancellationToken = default)
    {
        while (await _channel.Writer.WaitToWriteAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
        {
            if (_channel.Writer.TryWrite(filename))
            {
                _logger.LogInformation("{Filename} written to channel", filename);
                return true;
            }
        }
        return false;
    }

    public IAsyncEnumerable<string> ReadAll(CancellationToken cancellation = default)
    {
        return _channel.Reader.ReadAllAsync(cancellation);
    }

    public bool TryCompleteWriter(Exception? error = null)
    {
        return _channel.Writer.TryComplete(error);
    }

}

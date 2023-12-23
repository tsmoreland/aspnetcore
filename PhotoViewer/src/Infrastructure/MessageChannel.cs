//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Security.Cryptography;
using System.Threading.Channels;
using PhotoViewer.Shared;

namespace PhotoViewer.Infrastructure;

public class MessageChannel : IMessageChannel, IDisposable, IAsyncDisposable
{
    private readonly Channel<Message> _channel = Channel.CreateBounded<Message>(new BoundedChannelOptions(1) { FullMode = BoundedChannelFullMode.DropOldest });
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Task _messageSender;
    private bool _disposed;

    public MessageChannel()
    {
        _messageSender = Task.Factory.StartNew(ReadNotifications, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);
    }

    /// <inheritdoc />
    public bool NotifyDirectoryChange(string directory)
    {
        return _channel.Writer.TryWrite(new Message(MessageType.DirectoryChange,  directory));
    }

    /// <inheritdoc />
    public bool NotifyNavigationForward()
    {
        return _channel.Writer.TryWrite(new Message(MessageType.NavigationForward, string.Empty));
    }

    /// <inheritdoc />
    public bool NotifyNavigationBackward()
    {
        return _channel.Writer.TryWrite(new Message(MessageType.NavigationBackward, string.Empty));
    }


    /// <inheritdoc />
    public event EventHandler<string>? DirectoryChanged;

    /// <inheritdoc />
    public event EventHandler? MoveForward;

    /// <inheritdoc />
    public event EventHandler? MoveBackward;

    private async Task ReadNotifications(object? state)
    {
        if (state is not CancellationToken cancellationToken)
        {
            return;
        }

        await foreach (Message message in _channel.Reader.ReadAllAsync(cancellationToken))
        {
            switch (message.Type)
            {
                case MessageType.DirectoryChange:
                    DirectoryChanged?.Invoke(this, message.Data);
                    break;
                case MessageType.NavigationForward:
                    MoveForward?.Invoke(this, EventArgs.Empty);
                    break;
                case MessageType.NavigationBackward:
                    MoveBackward?.Invoke(this, EventArgs.Empty);
                    break;
                default:
                    throw new InvalidOperationException("Unrecognized message type");
            }

        }

    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        GC.SuppressFinalize(this);

        _cancellationTokenSource.Cancel(false);
        _channel.Writer.Complete();
        await _channel.Reader.Completion;
        await _messageSender;
        _cancellationTokenSource.Dispose();
    }
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _ = disposing;

        _cancellationTokenSource.Cancel(false);
        _channel.Writer.Complete();
        _channel.Reader.Completion.Wait(TimeSpan.FromMilliseconds(50));
        _messageSender.Wait(TimeSpan.FromMilliseconds(50));
        _messageSender.Dispose();
        _cancellationTokenSource.Cancel();
    }


}

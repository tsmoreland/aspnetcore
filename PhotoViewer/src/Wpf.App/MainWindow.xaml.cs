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

using System.IO;
using System.Windows;
using PhotoViewer.Shared;

namespace PhotoViewer.Wpf.App;

/// <summary>
/// Interaction logic for MainWindow
/// </summary>
public partial class MainWindow : Window
{
    private readonly IMessageChannel _messageChannel;
    private readonly System.Threading.Timer _timer;
    private bool _timerDisposed;

    public MainWindow(IMessageChannel messageChannel)
    {
        InitializeComponent();
        _messageChannel = messageChannel;

        PreviewKeyDown += MainWindow_PreviewKeyDown;
        PreviewMouseDown += MainWindow_PreviewMouseDown;
        Closing += MainWindow_Closing;
        _messageChannel.FileChanged += MessageChannel_FileChanged;

        _timer = new System.Threading.Timer(Timer_Callback, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (e.Key)
        {
            case System.Windows.Input.Key.Left:
                UpdateTimer(TimeSpan.FromSeconds(3));
                _messageChannel.NotifyNavigationBackward();
                break;
            case System.Windows.Input.Key.Escape:
                Toolbar.Visibility = Toolbar.Visibility != Visibility.Visible
                    ? Visibility.Visible
                    : Visibility.Collapsed;
                break;
            case System.Windows.Input.Key.Right:
            default:
                UpdateTimer(TimeSpan.FromSeconds(3));
                _messageChannel.NotifyNavigationForward();
                break;
        }
    }
    private void Timer_Callback(object? state) => _messageChannel.NotifyNavigationForward();
    private void MainWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _messageChannel.NotifyNavigationForward();
    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new FolderBrowserDialog();
        DialogResult result = dialog.ShowDialog();
        if (result != System.Windows.Forms.DialogResult.OK)
        {
            return;
        }

        Toolbar.Visibility = Visibility.Collapsed;
        _ = _messageChannel.NotifyDirectoryChange(dialog.SelectedPath);
    }
    private async void MessageChannel_FileChanged(object? sender, string filename)
    {
        await Dispatcher.InvokeAsync(() => Title = Path.GetFileNameWithoutExtension(filename));
    }
    private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        _timerDisposed = true;
        _timer.Dispose();
    }

    private void PeriodicChange_Checked(object sender, RoutedEventArgs e) => UpdateTimer(TimeSpan.FromSeconds(3));
    private void PeriodicChange_Unchecked(object sender, RoutedEventArgs e) => UpdateTimer(Timeout.Infinite);
    private void UpdateTimer(TimeSpan duePeriod)
    {
        if (_timerDisposed)
        {
            return;
        }
        _timer.Change(duePeriod, duePeriod);
    }

    private void UpdateTimer(int duePeriod)
    {
        if (_timerDisposed)
        {
            return;
        }
        _timer.Change(duePeriod, duePeriod);
    }
}

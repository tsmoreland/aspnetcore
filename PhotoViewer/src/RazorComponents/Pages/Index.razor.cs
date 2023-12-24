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

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PhotoViewer.Shared;

namespace PhotoViewer.RazorComponents.Pages;

public partial class Index
{

    [Inject]
    private IJSRuntime JsRuntime { get; init; } = null!;

    [Inject]
    private IMessageChannel Notifier { get; init; } = null!;

    private readonly List<string> _filenames = [];
    private readonly Random _random = new();
    private int _index = 0;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender)
        {
            return;
        }

        Notifier.DirectoryChanged += Notifier_DirectoryChanged;
        Notifier.MoveForward += Notifier_MoveForward;
        Notifier.MoveBackward += Notifier_MoveBackward;
    }

    private async void Notifier_DirectoryChanged(object? sender, string directory)
    {
        List<string> files = [.. Directory.GetFiles(directory)];

        _filenames.Clear();
        while (files.Count > 0)
        {
            int index = _random.Next(0, files.Count);
            _filenames.Add(files[index]);
            files.RemoveAt(index);
        }

        await LoadImage();
    }
    private async void Notifier_MoveForward(object? sender, EventArgs e) 
    {
        _index++;
        await LoadImage();
    }
    private async void Notifier_MoveBackward(object? sender, EventArgs e)
    {
        _index--;
        await LoadImage();
    }


    private async Task LoadImage(CancellationToken cancellationToken = default)
    {
        if (!IsIndexValid())
        {
            return;
        }
        string filename = _filenames[_index];
        await using FileStream stream = new(filename, FileMode.Open, FileAccess.Read);
        DotNetStreamReference streamRef = new(stream);
        await JsRuntime.InvokeVoidAsync("setImageUsingStreaming", cancellationToken, "wallpaper", streamRef);
        Notifier.NotifyFileChanged(filename);
    }

    private bool IsIndexValid()
    {
        if (_filenames.Count == 0)
        {
            return false;
        }
        
        if (_index >= _filenames.Count)
        {
            _index = 0;
        }
        else if (_index < 0)
        {
            _index = _filenames.Count - 1;
        }
        return true;
    }
}

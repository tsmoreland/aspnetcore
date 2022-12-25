// 
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace GloboTicket.TicketManagement.UI.ApiClient.Contracts;

public interface ITokenRepository
{
    bool ContainsKey(string key);
    ValueTask<bool> ContainsKeyAsync(string key, CancellationToken cancellationToken);
    string GetToken(string key);
    ValueTask<string> GetTokenAsync(string key, CancellationToken cancellationToken);

    /// <summary>
    /// <see cref="GetToken(string)"/> and/or <see cref="AddOrSetToken(string, string)"/>
    /// </summary>
    public string this[string key]
    {
        get => GetToken(key);
        set => AddOrSetToken(key, value);
    }

    void AddOrSetToken(string key, string value);
    ValueTask AddOrSetTokenAsync(string key, string value, CancellationToken cancellationToken);

    void RemoveToken(string key);
    ValueTask RemoveTokenAsync(string key, CancellationToken cancellationToken);
}

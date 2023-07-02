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
using MediatR;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Configuration;
using TennisByTheSea.Domain.Contracts.Greetings.Queries;

namespace TennisByTheSea.Application.Features.Greetings.Queries;

public sealed class GetRandomGreetingQueryHandler : IRequestHandler<GetRandomGreetingQuery, (string Greeting, string GreetingColour)>
{
    private readonly GreetingOptions _options;

    public GetRandomGreetingQueryHandler(IOptions<GreetingOptions> options)
    {
        _options = options.Value;
        RandomNumberGenerator.GetBytes(sizeof(int));
    }

    /// <inheritdoc />
    public Task<(string Greeting, string GreetingColour)> Handle(GetRandomGreetingQuery request, CancellationToken cancellationToken)
    {
        (bool isForLogin, string? name) = request;
        return isForLogin
            ? GetLoginGreeting(name, cancellationToken)
            : GetGreeting(name, cancellationToken);
    }

    private Task<(string Greeting, string GreetingColour)> GetLoginGreeting(string? name, CancellationToken cancellationToken)
    {
        int index = GetRandomNumber(_options.LoginGreetings.Count);
        return GetGreetingContentFromCollection(index, _options.LoginGreetings, name, cancellationToken);
    }

    private Task<(string Greeting, string GreetingColour)> GetGreeting(string? name, CancellationToken cancellationToken)
    {
        int index = GetRandomNumber(_options.Greetings.Count);
        return GetGreetingContentFromCollection(index, _options.Greetings, name, cancellationToken);
    }

    private Task<(string Greeting, string GreetingColour)> GetGreetingContentFromCollection(int index,
        IReadOnlyList<string> greetings, string? name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (index < 0 || index >= greetings.Count)
        {
            Task.FromResult<(string Greeting, string GreetingColour)>(("No Greeting found", _options.Colour));
        }

        string greeting = greetings[index];
        if (name is { Length: > 0 })
        {
            greeting = greeting.Replace("{name}", name);
        }
        return Task.FromResult<(string Greeting, string GreetingColour)>((greeting, _options.Colour));
    }

    private static int GetRandomNumber(int maximum)
    {
        Span<byte> data = stackalloc byte[sizeof(uint)];
        RandomNumberGenerator.Fill(data);

        if (BitConverter.IsLittleEndian)
        {
            data.Reverse();
        }

        uint value = BitConverter.ToUInt32(data);
        double ratio = uint.MaxValue / (double)Convert.ToUInt32(maximum);

        return Convert.ToInt32(value * ratio);
    }
}

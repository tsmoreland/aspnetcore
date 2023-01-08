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

using System.Net.Http.Headers;
using GloboTicket.TicketManagement.UI.ApiClient.Contracts;

namespace GloboTicket.TicketManagement.UI.ApiClient.Services;

public class BaseDataService
{
    protected IClient Client { get; }
    protected ITokenRepository TokenRepository { get; }

    public BaseDataService(IClient client, ITokenRepository tokenRepository)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        TokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
    }

    /*
    protected T ConvertApiExceptions<T><Guid>(ApiException ex, Func<string, bool, T> buildResponse)
    {
        if (ex.StatusCode == 400)
        {
            return new ApiResponse<Guid>() { Message = "Validation errors have occured.", ValidationErrors = ex.Response, Success = false };
        }
        else if (ex.StatusCode == 404)
        {
            return new ApiResponse<Guid>() { Message = "The requested item could not be found.", Success = false };
        }
        else
        {
            return new ApiResponse<Guid>() { Message = "Something went wrong, please try again.", Success = false };
        }
    }
    */

    protected async Task AddBearerToken(CancellationToken cancellationToken)
    {
        if (await TokenRepository.ContainsKeyAsync("token", cancellationToken))
        {
            Client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await TokenRepository.GetTokenAsync("token", cancellationToken));
        }
    }

}

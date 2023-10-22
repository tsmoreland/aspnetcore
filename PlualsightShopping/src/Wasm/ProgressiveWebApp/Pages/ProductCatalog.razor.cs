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

using System.Net.Http.Json;
using PlualsightShopping.Shared.DataTransferObjects;

namespace PluralsightShopping.Wasm.ProgressiveWebApp.Pages;

public partial class ProductCatalog
{
    private readonly HttpClient _client;
    public List<ProductDto> Products { get; init; } = new();

    /// <inheritdoc />
    public ProductCatalog(HttpClient client)
    {
        _client = client;
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        Products.Clear();
        Products.AddRange(await GetProducts().ToListAsync());
    }

    public async IAsyncEnumerable<ProductDto> GetProducts()
    {
        // NET8 adds GetFromJsonAsAsyncEnumerable
        ProductDto[]? products = await _client.GetFromJsonAsync<ProductDto[]>("");

        foreach (ProductDto product in products ?? Array.Empty<ProductDto>())
        {
            yield return product;
        }
    }
}

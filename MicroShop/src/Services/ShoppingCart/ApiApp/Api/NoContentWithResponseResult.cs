using System.Net;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.ShoppingCart.ApiApp.Api;

public sealed class NoContentWithResponseResult(ResponseDto value) : StatusCodeWithResponseResult(HttpStatusCode.NoContent, value)
{
    public static NoContentWithResponseResult Success() => new(ResponseDto.Ok());

    public static NoContentWithResponseResult Failure(string? errorMessage) => new(ResponseDto.Error(errorMessage ?? "Unknown error occurred"));

}

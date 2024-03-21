namespace MicroShop.Web.App.Models;

public record class ResponseDto<T>(T? Data, bool Success = true, string? ErrorMessage = null);

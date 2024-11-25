using GloboTicket.Shop.Shared.Api.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GloboTicket.Shop.Shared.Api.Filters;

public sealed class AddModelStateFeatureFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var modelState = context.ModelState;
        context.HttpContext.Features.Set(new ModelStateContainer(modelState));
        await next();
    }
}

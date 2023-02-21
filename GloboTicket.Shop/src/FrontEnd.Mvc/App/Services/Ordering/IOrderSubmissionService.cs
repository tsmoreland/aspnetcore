using GloboTicket.FrontEnd.Mvc.App.Models.View;

namespace GloboTicket.FrontEnd.Mvc.App.Services.Ordering;

public interface IOrderSubmissionService
{
    Task<Guid> SubmitOrder(CheckoutViewModel checkoutViewModel, CancellationToken cancellationToken);
}

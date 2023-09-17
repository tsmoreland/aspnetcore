namespace FlightPlan.Application.Contracts.Persistence;

public enum TransactionResult
{
    Success,
    BadRequest,
    NotFound,
    ServerError,
}

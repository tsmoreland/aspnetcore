namespace FlightPlan.Api.App.Authentication;

/// <summary>
/// DTO for user id and name
/// </summary>
/// <param name="Id">User Id</param>
/// <param name="Username">Username</param>
public record class User(Guid Id, string Username);


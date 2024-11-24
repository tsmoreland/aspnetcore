namespace FlightPlan.Api.App.Authentication;

/// <summary>
/// User Authentication service
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Returns user if <paramref name="username"/> and <paramref name="password"/> are correct
    /// </summary>
    /// <param name="username">username of user to authorize</param>
    /// <param name="password">password for that user</param>
    /// <returns>
    /// an asynchronous task which upon completion contains the authenticated user or
    /// <see langword="null"/> if  user does not exist or password is incorrect
    /// </returns>
    Task<User?> Authenticate(string username, string password);
}

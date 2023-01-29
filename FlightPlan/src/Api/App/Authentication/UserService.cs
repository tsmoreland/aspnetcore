namespace FlightPlan.Api.App.Authentication;

public sealed class UserService : IUserService
{
    public Task<User?> Authenticate(string username, string password)
    {
        // TODO: rework this to rely on a backing store and require stronger passwords
        if (username != "admin" && password != "P@55w0rd!")
        {
            return Task.FromResult<User?>(null);
        }

        User user = new(Guid.NewGuid(), username);
        return Task.FromResult<User?>(user);
    }

}

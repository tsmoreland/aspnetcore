using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SunDoeCoffeeShop.Shared.AuthPersistence;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App.Pages.Users;

public class IndexModel : PageModel
{
    private readonly IUserRepository _userRepository;
    public IList<UserModel>? Users { get; private set; }
    public string EmailSort { get; set; } = string.Empty;
    public string? Filter { get; set; }

    /// <inheritdoc />
    public IndexModel(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task OnGetAsync(string? sortOrder = null)
    {
        EmailSort = sortOrder is not { Length: > 0 } ? "Email DESC" : "Email";

        bool descending = EmailSort == "Email DESC";

        Users = await _userRepository.GetAll(descending, default).Select(i => new UserModel(i)).ToListAsync();
    }
}

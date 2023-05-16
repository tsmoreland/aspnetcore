using Microsoft.AspNetCore.Mvc.RazorPages;
using SunDoeCoffeeShop.Shared.AuthPersistence.Contracts;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App.Pages.Uesrs;

/// <summary/>
public class IndexModel : PageModel
{
    private readonly IUserRepository _userRepository;
    public IList<UserModel>? Users { get; private set; }
    public string UserNameSort { get; set; } = string.Empty;
    public string EmailSort { get; set; } = string.Empty;
    public string? Sort { get; set; }
    public string? Filter { get; set; }

    /// <inheritdoc />
    public IndexModel(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary/>
    public async Task OnGetAsync()
    {
        Users = await _userRepository.GetAll(default).Select(idUser => new UserModel(idUser)).ToListAsync();
    }
}

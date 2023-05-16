using Microsoft.AspNetCore.Mvc.RazorPages;
using SunDoeCoffeeShop.Shared.AuthPersistence;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App.Pages.Uesrs;

/// <summary/>
public class IndexModel : PageModel
{
    private readonly IUserRepository _userRepository;
    public List<UserModel>? Users { get; private set; } 

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

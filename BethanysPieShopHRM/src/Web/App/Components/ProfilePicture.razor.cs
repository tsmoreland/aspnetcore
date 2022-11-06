using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.Web.App.Components;

public partial class ProfilePicture
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; } 
}

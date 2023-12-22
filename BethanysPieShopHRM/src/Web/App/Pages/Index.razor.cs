using BethanysPieShopHRM.Web.App.Components.Widgets;

namespace BethanysPieShopHRM.Web.App.Pages;

public partial class Index
{
    public List<Type> Widgets { get; set; } =
    [
        typeof(EmployeeCountWidget),
        typeof(InboxWidget),
    ];
}

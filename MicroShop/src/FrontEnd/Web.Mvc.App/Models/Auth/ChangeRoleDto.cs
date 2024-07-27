using System.ComponentModel.DataAnnotations;

namespace MicroShop.Web.Mvc.App.Models.Auth;

public sealed record class ChangeRoleDto(
    [property: EmailAddress, Required] string Email,
    [property: Required] string RoleName);

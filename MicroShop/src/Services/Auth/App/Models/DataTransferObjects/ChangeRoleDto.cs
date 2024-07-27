using System.ComponentModel.DataAnnotations;

namespace MicroShop.Services.Auth.App.Models.DataTransferObjects;

public sealed record class ChangeRoleDto(
    [property: EmailAddress, Required] string Email,
    [property: Required] string RoleName);

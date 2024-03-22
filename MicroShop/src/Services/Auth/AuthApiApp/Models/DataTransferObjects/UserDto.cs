namespace MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

public sealed record class UserDto(string Id, string Email, string Name, string PhoneNumber)
{
    public UserDto(AppUser model)
        : this(model.Id, model.Email ?? string.Empty, model.Name, model.PhoneNumber ?? string.Empty)
    {

    }

    public AppUser ToAppUser()
    {
        return new AppUser
        {
            Id = Id,
            Email = Email,
            Name = Name,
            PhoneNumber = PhoneNumber,
        };
    }

}

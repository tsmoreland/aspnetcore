using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Ingredient
{
    private string _name;
    private string _amount;

    public Guid Id { get; }
    public string Name 
    {
        get => _name;
        set => _name = NameValidator.Instance.ValidateOrThrow(value);
    }

    public string Ammount
    {
        get => _amount;
        set => _amount = NameValidator.Instance.ValidateOrThrow(value); // TODO change to amount validator
    }
}

using BethanysPieShop.Admin.Domain.Validation;

namespace BethanysPieShop.Admin.Domain.Models;

public sealed class Ingredient
{
    private string _name;
    private string _amount;

    public Ingredient(string name, string amount)
        : this(Guid.NewGuid(), name, amount)
    {
    }
    private Ingredient(Guid id, string name, string amount)
    {
        _name = name;
        _amount = amount;
        Id = id;
    }

    public Guid Id { get; }
    public string Name 
    {
        get => _name;
        set => _name = NameValidator.Instance.ValidateOrThrow(value);
    }

    public string Amount
    {
        get => _amount;
        set => _amount = IngredientAmountValidator.Instance.ValidateOrThrow(value); 
    }
}

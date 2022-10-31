using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Api.Shared;

public interface ICountryRepository
{
    IEnumerable<Country> GetAllCountries();
    Country? GetCountryById(int countryId);
}

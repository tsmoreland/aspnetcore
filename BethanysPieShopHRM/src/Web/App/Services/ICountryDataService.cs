using BethanysPieShopHRM.Shared.Domain;

namespace BethanysPieShopHRM.Web.App.Services;

public interface ICountryDataService
{
    Task<IEnumerable<Country>> GetAllCountries();
    Task<Country?> GetCountryuById(int countryId);
}

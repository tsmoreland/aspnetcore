using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class CountryRepository(AppDbContext appDbContext) : ICountryRepository
{
    public IEnumerable<Country> GetAllCountries()
    {
        return appDbContext.Countries;
    }

    public Country? GetCountryById(int countryId)
    {
        return appDbContext.Countries
            .AsNoTracking()
            .FirstOrDefault(c => c.CountryId == countryId);
    }
}

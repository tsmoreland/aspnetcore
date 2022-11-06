﻿using BethanysPieShopHRM.Api.App.Shared;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopHRM.Api.App.Infrastructure;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _appDbContext;

    public CountryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Country> GetAllCountries()
    {
        return _appDbContext.Countries;
    }

    public Country? GetCountryById(int countryId)
    {
        return _appDbContext.Countries
            .AsNoTracking()
            .FirstOrDefault(c => c.CountryId == countryId);
    }
}

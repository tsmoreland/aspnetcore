using BethanysPieShopHRM.Api.App.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController(ICountryRepository countryRepository) : Controller
{
    [HttpGet]
    public IActionResult GetCountries()
    {
        return Ok(countryRepository.GetAllCountries());
    }

    [HttpGet("{id}")]
    public IActionResult GetCountryById(int id)
    {
        return Ok(countryRepository.GetCountryById(id));
    }
}

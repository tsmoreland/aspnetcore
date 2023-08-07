using BethanysPieShopHRM.Api.App.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopHRM.Api.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet]
    public IActionResult GetCountries()
    {
        return Ok(_countryRepository.GetAllCountries());
    }

    [HttpGet("{id}")]
    public IActionResult GetCountryById(int id)
    {
        return Ok(_countryRepository.GetCountryById(id));
    }
}

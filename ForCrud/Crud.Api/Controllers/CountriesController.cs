using Crud.Core.DTOs;
using Crud.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesService _countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    [HttpGet("get-all-countries")]
    public async Task<ActionResult<List<CountryResponse>>> GetAllCountries()
    {
        var countries = await _countriesService.GetAllCountriesAsync();

        return Ok(countries);
    }

    [HttpPost("add-country")]
    public async Task<IActionResult> AddCountry([FromBody] CountryAddRequest? request)
    {
        var country = await _countriesService.AddCountryAsync(request);

        return Ok(country);
    }

    [HttpGet("get-country-by-id")]
    public async Task<IActionResult> GetCountryById([FromQuery] Guid? id)
    {
        var country = await _countriesService.GetCountryByIdAsync(id);

        if (country == null)
            return NotFound();

        return Ok(country);
    }

    [HttpGet("get-country-by-name")]
    public async Task<IActionResult> GetCountryByName([FromQuery] string? name)
    {
        var country = await _countriesService.GetCountryByNameAsync(name);

        if (country == null)
            return NotFound();

        return Ok(country);
    }
}
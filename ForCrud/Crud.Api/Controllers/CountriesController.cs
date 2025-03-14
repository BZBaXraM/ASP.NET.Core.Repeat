using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTOs;

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
    public IActionResult GetAllCountries()
    {
        var countries = _countriesService.GetAllCountries();
        
        return Ok(countries);
    }
    
    [HttpPost("add-country")]
    public IActionResult AddCountry([FromBody] CountryAddRequest? request)
    {
        var country = _countriesService.AddCountry(request);
        
        if (country == null)
            return BadRequest();
        
        return Ok(country);
    }
    
    [HttpGet("get-country-by-id")]
    public IActionResult GetCountryById([FromQuery] Guid? id)
    {
        var country = _countriesService.GetCountryById(id);
        
        if (country == null)
            return NotFound();
        
        return Ok(country);
    }
}
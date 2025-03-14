using Enitites;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CityWeathersApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;


    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("get-all")]
    public ActionResult<List<CityWeather>> Get()
    {
        return _weatherService.GetWeathers();
    }

    [HttpGet("get-by-code/{code}")] // 
    public ActionResult<CityWeather> GetByCode(string code)
    {
        return _weatherService.GetByCode(code);
    }
}
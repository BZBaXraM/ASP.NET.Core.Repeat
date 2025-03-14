using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DiExample.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;
    private readonly ICitiesService _citiesService2;
    private readonly ICitiesService _citiesService3;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HomeController(ICitiesService citiesService, ICitiesService citiesService2, ICitiesService citiesService3,
        IServiceScopeFactory serviceScopeFactory)
    {
        _citiesService = citiesService;
        _citiesService2 = citiesService2;
        _citiesService3 = citiesService3;
        _serviceScopeFactory = serviceScopeFactory;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        var cities = _citiesService.GetCities();

        ViewBag.InstanceId_Cities_1 = _citiesService.ServiceInstanceId;

        ViewBag.InstanceId_Cities_2 = _citiesService2.ServiceInstanceId;

        ViewBag.InstanceId_Cities_3 = _citiesService3.ServiceInstanceId;

        using var scope = _serviceScopeFactory.CreateScope();

        var citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();

        ViewBag.InstanceId_Cities_1_Scope = citiesService.ServiceInstanceId;
        return View(cities);
    }
}
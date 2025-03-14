using Microsoft.AspNetCore.Mvc;
using Stocks.Api.Services;

namespace Stocks.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly IFinnHubService _finnHubService;

    public HomeController(IFinnHubService finnHubService)
    {
        _finnHubService = finnHubService;
    }

    [HttpGet("get-stock-price")]
    public async Task<IActionResult> GetStockPrice([FromQuery] string stock)
    {
        var data = await _finnHubService.GetStockPriceAsync(stock);
        return Ok(data);
    }

    [HttpGet("get-company-profile")]
    public async Task<IActionResult> GetCompanyProfile([FromQuery] string stock)
    {
        var data = await _finnHubService.GetCompanyProfileAsync(stock);
        return Ok(data);
    }
}
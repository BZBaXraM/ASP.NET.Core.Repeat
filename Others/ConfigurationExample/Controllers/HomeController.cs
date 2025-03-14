using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly ApiOptions _apiOptions;

    public HomeController(IOptions<ApiOptions> options)
    {
        _apiOptions = options.Value;
    }

    [HttpGet("get-client-id")]
    public IActionResult GetId()
    {
        return Ok(_apiOptions.ClientId);
    }

    [HttpGet("get-client-secret")]
    public IActionResult GetSecret()
    {
        return Ok(_apiOptions.ClientSecret); 
    }
}
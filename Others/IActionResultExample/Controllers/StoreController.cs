using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;

public class StoreController : Controller
{
    // GET: StoreController
    [HttpGet("store/books/{id}")]
    public IActionResult Books()
    {
        var id = Convert.ToInt32(Request.RouteValues["id"]);
        return Ok($"Books - {id}");
    }

}
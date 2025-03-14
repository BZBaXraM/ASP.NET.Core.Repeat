using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers;

public class HomeController : Controller
{
    // GET: HomeController
    [HttpGet("register")]
    public IActionResult Index(Person person)
    {
        if (!ModelState.IsValid)
        {
            string errors = string.Join("\n",
                ModelState
                    .Values.SelectMany(x => x.Errors)
                    .Select(err => err.ErrorMessage).ToList());

            return BadRequest(errors);
        }

        return Content($"{person}");
    }
}
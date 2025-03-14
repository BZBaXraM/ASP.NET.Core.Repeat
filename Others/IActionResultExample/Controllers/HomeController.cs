using IActionResultExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;

public class HomeController : Controller
{
    // bookstore?bookId=7&isLoggedIn=true - [FromQuery]
    // bookstore/1/true - [FromRoute]   
    [HttpGet("bookstore/{bookId:int?}/{isLoggedIn:bool?}")]
    public IActionResult Index([FromRoute] int? bookId, [FromRoute] bool? isLoggedIn, Book book)
    {
        if (bookId.HasValue is false)
        {
            return Content("Book id is not supplied");
        }

        if (bookId <= 0)
        {
            return BadRequest("Book Id can't be less than or equal to zero");
        }

        switch (bookId)
        {
            case <= 0:
                return BadRequest("Book id can't be less then or equal to zero");
            case > 1000:
                return NotFound("Book id can't be greater than 1000");
        }

        if (isLoggedIn == false)
        {
            return Unauthorized("User must be authenticated");
        }

        return Content($"Book id: {bookId}");

        // return RedirectToAction("Books", "Store", new { id = bookId });
        // return RedirectToActionPermanent("Books", "Store", new { id = bookId });

        // return LocalRedirect($"store/books/{bookId}");

        // return RedirectPermanent($"store/books/{bookId}");
    }
}
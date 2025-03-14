using BankControllerTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankControllerTask.Controllers;

public class HomeController : Controller
{
    // GET: HomeController
    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        return await Task.FromResult(Content("Welcome to ASP Bank!"));
    }

    [HttpGet("account-details")]
    public IActionResult Details()
    {
        BankInfo info = new()
        {
            AccountNumber = 1,
            AccountHolderName = "John Doe",
            CurrentBalance = 1000
        };

        return Json(info);
    }

    [HttpGet("account-statement")]
    public IActionResult Statement()
    {
        return File("/cv.pdf", "application/pdf");
    }

    [HttpGet("get-current-balance/{accountNumber:int?}")]
    public IActionResult GetCurrentBalance(int? accountNumber)
    {
        if (!accountNumber.HasValue)
        {
            return NotFound("Account Number should be supplied");
        }

        if (accountNumber != 1001)
        {
            return NotFound("Account number not found");
        }

        if (accountNumber != 1001)
        {
            return BadRequest("Account Number should be 1001");
        }

        return Ok(5000);
    }
}
using ControllersExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers;

[Controller]
public class HomeController : ControllerBase
{
    // GET: HomeController
    [HttpGet("/")]
    public async Task<string> Index()
    {
        return await Task.FromResult<string>("Hello from Index");
    }

    [HttpGet("about")]
    public async Task<string> About()
    {
        return await Task.FromResult<string>("Hello from About");
    }

    [HttpGet("person")]
    public async Task<JsonResult> Person()
    {
        Person person = new()
        {
            Id = Guid.NewGuid(),
            Name = "Salam Salamzade"
        };

        return await Task.FromResult(new JsonResult(person));
    }

    [HttpGet("contact-us/{mobile:regex(^\\d{{10}}$)}")]
    public async Task<string> Contact(string mobile)
    {
        return await Task.FromResult<string>($"Hello from Contact - {mobile}");
    }

    [HttpGet("file-download")]
    public async Task<IActionResult> FileDownload()
    {
        // return await Task.FromResult(new VirtualFileResult("/cv.pdf", "application/pdf"));
        return await Task.FromResult(File("/cv.pdf", "application/pdf"));
    }

    [HttpGet("file-download2")]
    public async Task<IActionResult> FileDownload2()
    {
        return await Task.FromResult(File(@"/Users/baxram/Documents/For Programming/cv.pdf",
            "application/pdf"));
    }

    [HttpGet("file-download3")]
    public async Task<IActionResult> FileDownload3()
    {
        var bytes = await System.IO.File.ReadAllBytesAsync("/Users/baxram/Documents/For Programming/cv.pdf");

        return await Task.FromResult(File(bytes,
            "application/pdf"));
    }
}
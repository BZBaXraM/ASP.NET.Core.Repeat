using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTOs;

namespace Crud.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonsService _personsService;

    public PersonsController(IPersonsService personsService)
    {
        _personsService = personsService;
    }

    [HttpGet("get-all-persons")]
    public IActionResult GetAllPersons()
    {
        var persons = _personsService.GetAllPersons();

        return Ok(persons);
    }

    [HttpPost("add-person")]
    public IActionResult AddPerson([FromBody] PersonAddRequest? request)
    {
        var person = _personsService.AddPerson(request);

        if (person == null)
            return BadRequest();

        return Ok(person);
    }

    [HttpGet("get-person-by-id")]
    public IActionResult GetPersonById([FromQuery] Guid? id)
    {
        var person = _personsService.GetPersonByPersonId(id);

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpGet("get-filtered-persons")]
    public IActionResult GetFilteredPersons([FromQuery] string searchBy, [FromQuery] string? searchString)
    {
        var persons = _personsService.GetFilteredPersons(searchBy, searchString);

        return Ok(persons);
    }

    [HttpPut("update-person")]
    public IActionResult UpdatePerson([FromBody] PersonUpdateRequest? request)
    {
        var person = _personsService.UpdatePerson(request);

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpDelete("delete-person")]
    public IActionResult DeletePerson([FromQuery] Guid? id)
    {
        var isDeleted = _personsService.DeletePerson(id);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
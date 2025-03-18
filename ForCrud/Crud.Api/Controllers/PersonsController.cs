using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTOs;
using ServiceContracts.Enums;

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
    public async Task<IActionResult> GetAllPersons()
    {
        var persons = await _personsService.GetAllPersonsAsync();

        return Ok(persons);
    }

    [HttpPost("add-person")]
    public async Task<IActionResult> AddPerson([FromBody] PersonAddRequest? request)
    {
        var person = await _personsService.AddPersonAsync(request);

        return Ok(person);
    }

    [HttpGet("get-person-by-id")]
    public async Task<IActionResult> GetPersonById([FromQuery] Guid? id)
    {
        var person = await _personsService.GetPersonByPersonIdAsync(id);

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpGet("get-filtered-persons")]
    public async Task<IActionResult> GetFilteredPersons([FromQuery] string searchBy, [FromQuery] string? searchString)
    {
        var persons = await _personsService.GetFilteredPersonsAsync(searchBy, searchString);

        return Ok(persons);
    }

    [HttpGet("get-sorted-persons")]
    public async Task<IActionResult> GetSortedPersons([FromQuery] string sortBy, [FromQuery] SortOrderOptions sortOrder)
    {
        var persons = await _personsService.GetAllPersonsAsync();

        var sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

        return Ok(sortedPersons);
    }

    [HttpPut("update-person")]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonUpdateRequest? request)
    {
        var person = await _personsService.UpdatePersonAsync(request);

        return Ok(person);
    }

    [HttpDelete("delete-person")]
    public async Task<IActionResult> DeletePerson([FromQuery] Guid? id)
    {
        var isDeleted = await _personsService.DeletePersonAsync(id);

        if (!isDeleted)
            return NotFound();

        return Ok();
    }
}
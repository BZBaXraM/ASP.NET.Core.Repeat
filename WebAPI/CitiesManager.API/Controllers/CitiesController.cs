using CitiesManager.API.Data;
using CitiesManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("get-all-cities")]
    public async Task<ActionResult<List<City>>> GetAll()
    {
        return Ok(await _context.Cities.ToListAsync());
    }

    [HttpGet("get-city/{id:guid}")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == id)!;

        if (city is null) return NotFound("City not found");

        return Ok(city);
    }

    [HttpPost("add-city")]
    public async Task<ActionResult<City>> AddCity(City city)
    {
        var newItem = await _context.Cities.AddAsync(city);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCity), new { id = newItem.Entity.CityId }, newItem.Entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<City>> PutCity(Guid id, City city)
    {
        var existingCity = await _context.Cities.FindAsync(id);

        if (existingCity is null) return NotFound();

        existingCity.CityName = city.CityName;

        await _context.SaveChangesAsync();

        return Ok(existingCity);
    }

    [HttpDelete("delete-city/{id:guid}")]
    public async Task<ActionResult<City>> DeleteCity(Guid id)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == id);

        if (city is null) return BadRequest();

        var deletedCity = _context.Cities.Remove(city);

        await _context.SaveChangesAsync();

        return Ok(deletedCity.Entity);
    }
}
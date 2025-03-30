using Crud.Core.Domain.Entities;
using Crud.Core.RepositoryContracts;
using Crud.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly ApplicationDbContext _context;

    public CountriesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Country>> GetAllCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country> AddCountryAsync(Country country)
    {
        await _context.Countries.AddAsync(country);
        
        await _context.SaveChangesAsync();
        
        return country;
    }

    public async Task<Country?> GetCountryByIdAsync(Guid id)
    {
        return await _context.Countries.FindAsync(id);
    }

    public async Task<Country?> GetCountryByNameAsync(string name)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.CountryName == name);
    }
}
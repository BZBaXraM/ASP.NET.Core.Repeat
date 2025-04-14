using Crud.Domain.Entities;

namespace Crud.Core.RepositoryContracts;

public interface ICountriesRepository
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<Country> AddCountryAsync(Country country);
    Task<Country?> GetCountryByIdAsync(Guid id);
    Task<Country?> GetCountryByNameAsync(string name);
}
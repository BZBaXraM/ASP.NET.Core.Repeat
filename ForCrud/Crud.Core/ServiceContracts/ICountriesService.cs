using Crud.Core.DTOs;

namespace Crud.Core.ServiceContracts;

public interface ICountriesService
{
    Task<List<CountryResponse>> GetAllCountriesAsync();
    Task<CountryResponse> AddCountryAsync(CountryAddRequest? request);
    Task<CountryResponse?> GetCountryByIdAsync(Guid? id);
    Task<CountryResponse?> GetCountryByNameAsync(string? name);
}
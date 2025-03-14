using ServiceContracts.DTOs;

namespace ServiceContracts;

public interface ICountriesService
{
    List<CountryResponse> GetAllCountries();
    CountryResponse? AddCountry(CountryAddRequest? request);
    CountryResponse? GetCountryById(Guid? id);
}
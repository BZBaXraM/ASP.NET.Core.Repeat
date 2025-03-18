using Entities;
using Entities.Data;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTOs;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository;
    }

    public async Task<List<CountryResponse>> GetAllCountriesAsync()
    {
        var countries = await _countriesRepository.GetAllCountriesAsync();

        return countries.Select(temp => new CountryResponse
        {
            CountryId = temp.CountryId,
            CountryName = temp.CountryName
        }).ToList();
    }

    public async Task<CountryResponse> AddCountryAsync(CountryAddRequest? countryAddRequest)
    {
        ArgumentNullException.ThrowIfNull(countryAddRequest);

        var country = new Country
        {
            CountryName = countryAddRequest.CountryName
        };

        var addedCountry = await _countriesRepository.AddCountryAsync(country);

        return new CountryResponse
        {
            CountryId = addedCountry.CountryId,
            CountryName = addedCountry.CountryName
        };
    }

    public async Task<CountryResponse?> GetCountryByIdAsync(Guid? id)
    {
        if (id is null)
        {
            return null;
        }

        var country = await _countriesRepository.GetCountryByIdAsync(id.Value);

        return country is null
            ? null
            : new CountryResponse
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
    }

    public async Task<CountryResponse?> GetCountryByNameAsync(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var country = await _countriesRepository.GetCountryByNameAsync(name);

        return country is null
            ? null
            : new CountryResponse
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
    }
}
using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = [];
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(x => x.ToCountryResponse()).ToList();
    }

    public CountryResponse? AddCountry(CountryAddRequest? request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.CountryName == null)
        {
            throw new ArgumentException(nameof(request.CountryName));
        }

        if (_countries.Any(temp => temp.CountryName == request.CountryName))
        {
            throw new ArgumentException("Given country name already exists");
        }

        var country = request.ToCountry();

        country.CountryId = Guid.NewGuid();

        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public CountryResponse? GetCountryById(Guid? id)
    {
        if (id == null)
            return null;

        var countryResponseFromList = _countries.FirstOrDefault(temp => temp.CountryId == id);

        return countryResponseFromList?.ToCountryResponse();
    }
}
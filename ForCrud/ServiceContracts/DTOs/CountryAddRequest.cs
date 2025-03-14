using Entities;

namespace ServiceContracts.DTOs;

public class CountryAddRequest
{
    public string? CountryName { get; set; }

    public Country ToCountry()
    {
        return new Country() { CountryName = CountryName };
    }
}

public static class CountryExtensions
{
    //Converts from Country object to CountryResponse object
    public static CountryResponse? ToCountryResponse(this Country country)
    {
        return new CountryResponse() { CountryId = country.CountryId, CountryName = country.CountryName };
    }
}
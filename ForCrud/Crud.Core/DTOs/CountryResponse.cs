namespace Crud.Core.DTOs;

public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(CountryResponse))
        {
            return false;
        }

        var countryToCompare = (CountryResponse)obj;

        return CountryId == countryToCompare.CountryId && CountryName == countryToCompare.CountryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
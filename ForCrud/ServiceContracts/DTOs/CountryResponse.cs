namespace ServiceContracts.DTOs;

public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    //It compares the current object to another object of CountryResponse type and returns true, if both values are same; otherwise returns false
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

        CountryResponse countryToCompare = (CountryResponse)obj;

        return CountryId == countryToCompare.CountryId && CountryName == countryToCompare.CountryName;
    }

    //returns an unique key for the current object
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
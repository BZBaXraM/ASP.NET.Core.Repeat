namespace CountriesApp.Models;

public class Country(int id, string? name)
{
    public int Id { get; set; } = id;
    public string? Name { get; set; } = name;
}
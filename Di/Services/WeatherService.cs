using Enitites;
using ServiceContracts;

namespace Services;

public class WeatherService : IWeatherService
{
    private readonly List<CityWeather> _weathers;

    public WeatherService()
    {
        _weathers =
        [
            new CityWeather
            {
                CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33
            },


            new CityWeather
            {
                CityUniqueCode = "NYC", CityName = "New York", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60
            },


            new CityWeather
            {
                CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82
            }
        ];
    }

    public List<CityWeather> GetWeathers()
    {
        return _weathers;
    }

    public CityWeather GetByCode(string code)
    {
        return _weathers.FirstOrDefault(x => x.CityUniqueCode == code)!;
    }
}
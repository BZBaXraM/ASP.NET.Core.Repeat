using Enitites;

namespace ServiceContracts;

public interface IWeatherService
{
    List<CityWeather> GetWeathers();
    CityWeather GetByCode(string code);
}
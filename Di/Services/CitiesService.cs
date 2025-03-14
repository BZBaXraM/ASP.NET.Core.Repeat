using ServiceContracts;

namespace Services;

public class CitiesService : ICitiesService, IDisposable
{
    private readonly List<string> _cities;

    public CitiesService()
    {
        ServiceInstanceId = Guid.NewGuid();
        _cities =
        [
            "Baku",
            "Moscow",
            "Istanbul",
            "New York",
            "London"
        ];
    }

    public Guid ServiceInstanceId { get; }

    public List<string> GetCities()
    {
        return _cities;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
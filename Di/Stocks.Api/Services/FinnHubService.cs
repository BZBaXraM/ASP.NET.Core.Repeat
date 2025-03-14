using Stocks.Api.Models;

namespace Stocks.Api.Services;

public class FinnHubService : IFinnHubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FinnHubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<RootObject?> GetStockPriceAsync(string stock)
    {
        using var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(
            $"{_configuration
                .GetSection("Api")["StockPriceUrl"]}{stock}&token={_configuration
                .GetSection("Api")["Key"]}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadFromJsonAsync<RootObject>();

        return content;
    }

    public async Task<CompanyProfile?> GetCompanyProfileAsync(string stock)
    {
        using var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(
            $"{_configuration
                .GetSection("Api")["CompanyProfileUrl"]}{stock}&token={_configuration
                .GetSection("Api")["Key"]}");

        if (!response.IsSuccessStatusCode) return null;
        var context = await response.Content.ReadFromJsonAsync<CompanyProfile>();

        return context;
    }
}
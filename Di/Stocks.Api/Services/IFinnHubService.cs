using Stocks.Api.Models;

namespace Stocks.Api.Services;

public interface IFinnHubService
{
    Task<RootObject?> GetStockPriceAsync(string stock);
    Task<CompanyProfile?> GetCompanyProfileAsync(string stock);
}
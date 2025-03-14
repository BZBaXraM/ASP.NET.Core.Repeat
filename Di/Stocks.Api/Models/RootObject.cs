namespace Stocks.Api.Models;

public class RootObject
{
    public double c { get; set; }
    public double d { get; set; }
    public double dp { get; set; }
    public double h { get; set; }
    public double l { get; set; }
    public double o { get; set; }
    public double pc { get; set; }
    public int t { get; set; }
}

public class CompanyProfile
{
    public string country { get; set; }
    public string currency { get; set; }
    public string estimateCurrency { get; set; }
    public string exchange { get; set; }
    public string finnhubIndustry { get; set; }
    public string ipo { get; set; }
    public string logo { get; set; }
    public double marketCapitalization { get; set; }
    public string name { get; set; }
    public string phone { get; set; }
    public double shareOutstanding { get; set; }
    public string ticker { get; set; }
    public string weburl { get; set; }
}
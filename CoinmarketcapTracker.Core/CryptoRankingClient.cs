using NoobsMuc.Coinmarketcap.Client;
using RestSharp.Portable;

namespace CoinmarketcapTracker.Core;

public class CryptoRankingClient
{
    private const string DefaultCurrency = "USD";

    private const string UrlBase = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/";

    private const string UrlPartListingLatest = "listings/latest";

    private const string UrlPartListingHistorical = "listings/historical";

    private const int MaxPageSize = 5000;

    private string _apiKey;

    protected virtual WebApiClient GetWebApiClientForLatest(Dictionary<string, string> queryArguments)
    {
        return this.GetWebApiClient(CryptoRankingClient.UrlPartListingLatest, queryArguments);
    }

    protected virtual WebApiClient GetWebApiClientForHistorical(Dictionary<string, string> queryArguments)
    {
        return this.GetWebApiClient(CryptoRankingClient.UrlPartListingHistorical, queryArguments);
    }

    protected virtual WebApiClient GetWebApiClient(string urlPart, Dictionary<string, string> queryArguments)
    {
        UriBuilder uriBuilder = new UriBuilder(CryptoRankingClient.UrlBase + urlPart);
        return new WebApiClient(uriBuilder, queryArguments, this._apiKey);
    }

    public virtual List<Currency> GetResultsPaging(
        Func<int, List<Currency>> pageRetriever,
        int howManyCurrencies = 10000)
    {
        ArgumentNullException.ThrowIfNull(pageRetriever);

        if (howManyCurrencies < 1)
        {
            throw new ArgumentNullException(nameof(howManyCurrencies), $"The \"{nameof(howManyCurrencies)}\" argument should be greater or equal to 1.");
        }

        List<Currency> allCurrencies = new List<Currency>();

        int rankingIndex = 1;
        while (rankingIndex < howManyCurrencies)
        {
            List<Currency> nextPage = pageRetriever(rankingIndex);
            if (!nextPage.Any())
            {
                break;
            }

            allCurrencies.AddRange(nextPage);
            rankingIndex += nextPage.Count;
        }

        return allCurrencies;
    }

    #region Retrieving the latest listing

    public virtual List<Currency> Latest(
        int start = 1, 
        int limit = CryptoRankingClient.MaxPageSize)
    {
        if (start < 1)
        {
            throw new ArgumentNullException(nameof(start), $"The \"{nameof(start)}\" argument should be greater or equal to 1.");
        }

        if (limit < 1 || limit > CryptoRankingClient.MaxPageSize)
        {
            throw new ArgumentNullException(nameof(limit), $"The \"{nameof(limit)}\" argument should be between 1 and 5000.");
        }

        string convert = CryptoRankingClient.DefaultCurrency;

        Dictionary<string, string> queryArguments = new()
        {
            { "start", start.ToString() },
            { "limit", limit.ToString() },
            { "convert", convert }
        };

        WebApiClient client = this.GetWebApiClientForLatest(queryArguments);
        return client.MakeRequest(Method.GET, convert, false, false, new List<string>());
    }

    public virtual List<Currency> Latest(int howManyCurrencies = 10000)
    {
        return this.GetResultsPaging(start => this.Latest(start, CryptoRankingClient.MaxPageSize), howManyCurrencies);
    }

    #endregion

    #region Retrieving historical listing

    public virtual List<Currency> Historical(
        DateOnly date, 
        int start = 1, 
        int limit = CryptoRankingClient.MaxPageSize)
    {
        if (start < 1)
        {
            throw new ArgumentNullException(nameof(start), $"The \"{nameof(start)}\" argument should be greater or equal to 1.");
        }

        if (limit < 1 || limit > CryptoRankingClient.MaxPageSize)
        {
            throw new ArgumentNullException(nameof(limit), $"The \"{nameof(limit)}\" argument should be between 1 and 5000.");
        }

        string convert = CryptoRankingClient.DefaultCurrency;

        Dictionary<string, string> queryArguments = new()
        {
            { "date", date.ToString("yyyy-MM-dd") },
            { "start", start.ToString() },
            { "limit", limit.ToString() },
            { "convert", convert }
        };

        WebApiClient client = this.GetWebApiClientForHistorical(queryArguments);
        return client.MakeRequest(Method.GET, convert, false, false, new List<string>());
    }

    public virtual List<Currency> Historical(DateOnly date, int howManyCurrencies = 10000)
    {
        return this.GetResultsPaging(start => this.Historical(date, start, CryptoRankingClient.MaxPageSize), howManyCurrencies);
    }

    #endregion

    public CryptoRankingClient(string apiKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));

        this._apiKey = apiKey;
    }
}
using CoinmarketcapTracker.Core;
using NoobsMuc.Coinmarketcap.Client;

try
{
    int returnTopCryptos = 10000;

    string myKey = "<Your Coinmarketcap API key>";

    CryptoRankingClient client = new CryptoRankingClient(myKey);

    Console.WriteLine($"Request for {returnTopCryptos} top cryptocurrencies from the Coinmarketcap API.");
    List<Currency> ranking = client.Latest(returnTopCryptos);
    Console.WriteLine($"Returned {ranking.Count} top cryptocurrencies from the Coinmarketcap API.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error occured on getting cryptocurrency ranking from the  Coinmarketcap API: {ex}.");
}
finally
{
    Console.WriteLine("The application is terminating.");
}
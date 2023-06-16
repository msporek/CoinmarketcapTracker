using CoinmarketcapTracker.Core;
using Newtonsoft.Json;
using NoobsMuc.Coinmarketcap.Client;
using System.Reflection;

try
{
    int returnTopCryptos = 10000;

    string myKey = "<Your Coinmarketcap API key>";

    CryptoRankingClient client = new CryptoRankingClient(myKey);

    Console.WriteLine($"Request for {returnTopCryptos} top cryptocurrencies from the Coinmarketcap API.");
    List<Currency> ranking = client.Latest(returnTopCryptos);
    Console.WriteLine($"Returned {ranking.Count} top cryptocurrencies from the Coinmarketcap API.");

    string storageFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "storage");
    if (!Directory.Exists(storageFolder))
    {
        Directory.CreateDirectory(storageFolder);
    }

    string rankingSerialized = JsonConvert.SerializeObject(ranking);

    string todayFileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}.json";
    string todayFilePath = Path.Combine(storageFolder, todayFileName);
    if (File.Exists(todayFilePath))
    {
        File.Delete(todayFilePath);
    }
    File.WriteAllText(todayFilePath, rankingSerialized);

    Console.WriteLine($"Successfully the cryptocurrency ranking in file: {todayFilePath}.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error occured on getting cryptocurrency ranking from the  Coinmarketcap API: {ex}.");
}
finally
{
    Console.WriteLine("The application is terminating.");
}
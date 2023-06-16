using CoinmarketcapTracker.Analytics;

try
{
    string storeFilesInLocation = @"C:\storage";

    string[] allFiles = Directory.GetFiles(storeFilesInLocation);
    Dictionary<DateOnly, string> dateToFilePath = new();
    foreach (string filePath in allFiles)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        if (DateOnly.TryParse(fileNameWithoutExtension, out DateOnly fileDate))
        {
            dateToFilePath[fileDate] = filePath;
        }
    }

    List<IAlgoRule> algoRules =
        new List<IAlgoRule>()
        {
            // TODO: Fill algo rules here. 
        };

    new Analyzer(dateToFilePath, algoRules).RunAnalyticalRules();
}
catch (Exception ex)
{
    Console.WriteLine($"Error occured on retrieving data stored in Coinmarketcap JSON result files: {ex}.");
}
finally
{
    Console.WriteLine("The application is terminating.");
}
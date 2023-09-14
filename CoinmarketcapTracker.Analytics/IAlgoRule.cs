using NoobsMuc.Coinmarketcap.Client;

namespace CoinmarketcapTracker.Analytics;

/// <summary>
/// Interface meant to be implemented by classes the provide the algorithmic rules functionality, 
/// running algorithmic rules against data and returning a result that indicates the amount of profit / loss. 
/// </summary>
public interface IAlgoRule
{
    public double Run(Dictionary<DateOnly, List<Currency>> dateToRanking);
}

using NoobsMuc.Coinmarketcap.Client;

namespace CoinmarketcapTracker.Analytics;

public interface IAlgoRule
{
    public double Run(Dictionary<DateOnly, List<Currency>> dateToRanking);
}

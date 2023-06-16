using NoobsMuc.Coinmarketcap.Client;

namespace CoinmarketcapTracker.Analytics;

public class SampleAlgoRule : IAlgoRule
{
    public virtual double Run(Dictionary<DateOnly, List<Currency>> dateToRanking)
    {
        // TODO: Rule implementation goes here. 
        throw new NotImplementedException();
    }

    public SampleAlgoRule()
    {
    }
}

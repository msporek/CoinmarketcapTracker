using Newtonsoft.Json;
using NoobsMuc.Coinmarketcap.Client;

namespace CoinmarketcapTracker.Analytics;

public class Analyzer
{
    private Dictionary<DateOnly, string> _dateToFilePath;

    private DateOnly _minDate;

    private DateOnly _maxDate;

    private IList<IAlgoRule> _algoRules;

    protected List<Currency> ParseForDate(DateOnly date)
    {
        if (this._dateToFilePath.TryGetValue(date, out string? filePath) &&
            !string.IsNullOrWhiteSpace(filePath))
        {
            List<Currency>? dateRanking = JsonConvert.DeserializeObject<List<Currency>>(File.ReadAllText(filePath!));
            if (dateRanking != null)
            {
                return dateRanking!;
            }
            else
            {
                return new List<Currency>();
            }
        }

        return new List<Currency>();
    }

    public double RunAnalyticalRules(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
        {
            (startDate, endDate) = (endDate, startDate);
        }

        if (startDate < this._minDate)
        {
            startDate = this._minDate;
        }

        if (endDate > this._maxDate)
        {
            endDate = this._maxDate;
        }

        Dictionary<DateOnly, List<Currency>> dateToRanking = new();
        for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
        {
            dateToRanking[date] = this.ParseForDate(date);
        }

        double cumulativeProfit = this._algoRules.Sum(r => r.Run(dateToRanking));
        return cumulativeProfit;
    }

    public double RunAnalyticalRules()
    {
        return this.RunAnalyticalRules(this._minDate, this._maxDate);
    }

    public Analyzer(
        Dictionary<DateOnly, string> dateToFilePath, 
        IList<IAlgoRule> algoRules)
    {
        ArgumentNullException.ThrowIfNull(dateToFilePath);
        ArgumentNullException.ThrowIfNull(algoRules);

        this._dateToFilePath = dateToFilePath;

        this._minDate = this._dateToFilePath.Keys.Min();
        this._maxDate = this._dateToFilePath.Keys.Max();

        this._algoRules = algoRules;
    }
}

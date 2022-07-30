namespace TRPG.DiceRoller.RollsResults;

public class DicePoolRollResult
{
    private delegate void OnChangedHandler();
    private event OnChangedHandler? Changed;

    private int _numberOfHighestRemoved = 0;
    private int _numberOfLowestRemoved = 0;

    private DiceRollResult[] OriginalResults { get; init; } = Array.Empty<DiceRollResult>();
    public DiceRollResult[] Results { get; private set; } = Array.Empty<DiceRollResult>();

    public int NumberOfHighestRemoved
    {
        get => _numberOfHighestRemoved;
        set
        {
            if (value < 0 || value + NumberOfLowestRemoved > OriginalResults.Length)
                throw new ArgumentOutOfRangeException(nameof(value));
            _numberOfHighestRemoved = value;
            Changed?.Invoke();
        }
    }
    public int NumberOfLowestRemoved
    {
        get => _numberOfLowestRemoved;
        set
        {
            if (value < 0 || value + NumberOfHighestRemoved > OriginalResults.Length)
                throw new ArgumentOutOfRangeException(nameof(value));
            _numberOfLowestRemoved = value;
            Changed?.Invoke();
        }
    }

    public DiceRollResult[] HighestRemoved { get; private set; } = Array.Empty<DiceRollResult>();
    public DiceRollResult[] LowestRemoved { get; private set; } = Array.Empty<DiceRollResult>();

    public int Sum { get; private set; }
    public DiceRollResult? HighestResult { get; private set; }
    public DiceRollResult? LowestResult { get; private set; }

    public DicePoolRollResult(DiceRollResult[] results)
    {
        OriginalResults = results.ToArray();

        Changed = OnChanged;
        Changed?.Invoke();
    }

    private void OnChanged()
    {
        Results = OriginalResults
            .OrderBy(r => r.Value)
            .ToArray();

        HighestRemoved = Results
            .TakeLast(NumberOfHighestRemoved)
            .OrderBy(r => r.Id)
            .ToArray();

        LowestRemoved = Results
            .Take(NumberOfLowestRemoved)
            .OrderBy(r => r.Id)
            .ToArray();

        Results = Results
            .Skip(NumberOfLowestRemoved)
            .SkipLast(NumberOfHighestRemoved)
            .OrderBy(r => r.Id)
            .ToArray();

        Sum = Results.Sum(r => r.Value);
        HighestResult = Results.MaxBy(r => r.Value);
        LowestResult = Results.MinBy(r => r.Value);
    }
}

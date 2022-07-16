namespace TRPG.DiceRoller.RollsResults;

public record DiceRollResult : IComparable<DiceRollResult>
{
    public Dice Dice { get; init; }
    public int Value { get; init; }

    public DiceRollResult(Dice dice, int value)
    {
        Dice = dice;
        Value = value;
    }

    public int CompareTo(DiceRollResult? other)
    {
        var diceCompareResult = Dice.CompareTo(other?.Dice);

        if (diceCompareResult == 0)
            return Value.CompareTo(other?.Value) * -1;

        return diceCompareResult;
    }
}

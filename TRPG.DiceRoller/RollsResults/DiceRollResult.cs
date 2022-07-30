namespace TRPG.DiceRoller.RollsResults;

public record DiceRollResult
{
    public int Id { get; init; }
    public Dice Dice { get; init; }
    public int Value { get; init; }

    public DiceRollResult(int id, Dice dice, int value)
    {
        Id = id;
        Dice = dice;
        Value = value;
    }
}

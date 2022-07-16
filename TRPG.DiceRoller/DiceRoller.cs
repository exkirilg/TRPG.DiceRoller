namespace TRPG.DiceRoller;

public class DiceRoller
{
    private readonly IRandomIntAdapter _randomIntAdapter;

    public DiceRoller(IRandomIntAdapter randomIntAdapter)
    {
        _randomIntAdapter = randomIntAdapter;
    }

    public DiceRollResult RollDice(Dice dice)
    {
        return new DiceRollResult(
            dice,
            _randomIntAdapter.GetRandomPositiveNumberAboveZeroInRange(dice.NumberOfSides));
    }

    public DicePoolRollResult RollDicePool(DicePool dicePool)
    {
        var rolls = new DiceRollResult[dicePool.Count];
        for (int i = 0; i < dicePool.Count; i++)
            rolls[i] = RollDice(dicePool[i]);

        return new DicePoolRollResult(rolls);
    }
}

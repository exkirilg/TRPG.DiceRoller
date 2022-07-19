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

    public SuccessRate CalculateSuccessRate(Dice dice, Func<int, bool> predicate)
    {
        double possibleOutcomes = dice.NumberOfSides;
        double desiredOutcomes = dice.SidesAsIntArray.Where(predicate).Count();

        return new(desiredOutcomes/possibleOutcomes);
    }

    public SuccessRate CalculateSuccessRate(DicePool dicePool, Func<int, bool> predicate)
    {
        double result = 1;

        foreach (var dice in dicePool)
            result *= CalculateSuccessRate(dice, predicate).Value;

        return new(result);
    }
}

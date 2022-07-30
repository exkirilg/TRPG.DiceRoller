using System.Text.RegularExpressions;

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

    public DicePoolRollResult RollDicePoolByExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException($"Expression cannot be parsed to dice pool.");

        var dices = new List<Dice>();

        var dicesTypes = typeof(D20).Assembly.GetTypes()
            .Where(t => t.Namespace!.Equals(typeof(D20).Namespace))
            .ToArray();

        var matches = new Regex("[/]?[0-9]*[dD][0-9]*").Matches(expression);
        foreach (var match in matches.Select(m => m.Value.Replace("/", string.Empty).ToUpper()))
        {
            var numArray = match.Split('D');

            _ = int.TryParse(numArray[0], out int numOfRolls);
            _ = int.TryParse(numArray[1], out int numOfSides);

            var type = dicesTypes.Where(t => t.Name.Equals($"D{numOfSides}")).FirstOrDefault();
            if (type is null)
                continue;

            for (int i = 0; i < Math.Max(numOfRolls, 1); i++)
            {
                var dice = Activator.CreateInstance(type) as Dice;
                dices.Add(dice!);
            }
        }

        if (dices.Any() == false)
            throw new ArgumentException($"Expression cannot be parsed to dice pool.");

        return RollDicePool(new DicePool(dices.ToArray()));
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

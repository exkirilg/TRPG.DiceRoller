namespace TRPG.DiceRoller.Adapters;

public class RandomIntAdapter : IRandomIntAdapter
{
    private readonly Random _random = new();
    public int GetRandomPositiveNumberAboveZeroInRange(int range) => _random.Next(1, range + 1);
}

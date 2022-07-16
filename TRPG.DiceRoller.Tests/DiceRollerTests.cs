namespace TRPG.DiceRoller.Tests;

public class DiceRollerTests
{
    private readonly DiceRoller _diceRoller;
    private readonly Mock<IRandomIntAdapter> _mockRandomIntAdapter;

    public static IEnumerable<object[]> RollDice_TestData => new List<object[]>()
    {
        new object[] { new D4(), 4 },
        new object[] { new D6(), 6 },
        new object[] { new D8(), 8 },
        new object[] { new D10(), 10 },
        new object[] { new D12(), 12 },
        new object[] { new D20(), 20 },
        new object[] { new D100(), 100 },
    };
    public static IEnumerable<object[]> RollDicePool_TestData => new List<object[]>
    {
        new object[]
        {
            new Dice[]
            {
                new D4(),
                new D4(),
                new D100(),
                new D6(),
                new D6()
            }
        },
        new object[]
        {
            new Dice[]
            {
                new D20(),
                new D8(),
                new D8(),
                new D100(),
                new D4(),
                new D6()
            }
        },
        new object[]
        {
            new Dice[]
            {
                new D12(),
                new D4(),
                new D4(),
                new D6(),
                new D6(),
                new D6(),
                new D10(),
                new D12(),
                new D20()
            }
        }
    };

    public DiceRollerTests()
    {
        _mockRandomIntAdapter = new Mock<IRandomIntAdapter>();
        _mockRandomIntAdapter
            .Setup(random => random.GetRandomPositiveNumberAboveZeroInRange(It.IsAny<int>()))
            .Returns((int range) => range);

        _diceRoller = new DiceRoller(_mockRandomIntAdapter.Object);
    }

    [Theory]
    [MemberData(nameof(RollDice_TestData))]
    public void RollDice_ValueInRange(Dice dice, int range)
    {
        var diceRoller = new DiceRoller(new RandomIntAdapter());

        for (int i = 100; i > 0; i--)
        {
            var roll = diceRoller.RollDice(dice);
            Assert.Equal(dice, roll.Dice);
            Assert.InRange(roll.Value, 1, range);
        }
    }

    [Theory]
    [MemberData(nameof(RollDice_TestData))]
    public void RollDice(Dice dice, int expected)
    {
        var roll = _diceRoller.RollDice(dice);

        Assert.Equal(dice, roll.Dice);
        Assert.Equal(expected, roll.Value);
    }

    [Theory]
    [MemberData(nameof(RollDicePool_TestData))]
    public void RollDicePool(Dice[] dices)
    {
        var dicePool = new DicePool(dices);
        var roll = _diceRoller.RollDicePool(dicePool);

        Assert.Equal(dices.Length, roll.Results.Length);
        Assert.Equal(dices.Sum(d => d.NumberOfSides), roll.Sum);
        Assert.Equal(dices.MaxBy(d => d.NumberOfSides)?.NumberOfSides, roll.HighestResult?.Value);
        Assert.Equal(dices.MinBy(d => d.NumberOfSides)?.NumberOfSides, roll.LowestResult?.Value);
    }

    [Theory]
    [MemberData(nameof(RollDicePool_TestData))]
    public void RollDicePool_WithRollsRemoved(Dice[] dices)
    {
        var dicePool = new DicePool(dices);
        var roll = _diceRoller.RollDicePool(dicePool);

        Assert.Equal(0, roll.NumberOfHighestRemoved);
        Assert.Equal(0, roll.NumberOfLowestRemoved);

        Assert.Empty(roll.HighestRemoved);
        Assert.Empty(roll.LowestRemoved);

        for (int i = 1; i <= dices.Length; i++)
        {
            var highest = roll.Results.MaxBy(r => r.Value);
            var lowest = roll.Results.MinBy(r => r.Value);

            if (i * 2 > dices.Length)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    roll.NumberOfHighestRemoved = i;
                    roll.NumberOfLowestRemoved = i;
                });

                break;
            }

            roll.NumberOfHighestRemoved = i;
            Assert.Equal(i, roll.NumberOfHighestRemoved);
            Assert.Equal(i, roll.HighestRemoved.Length);
            Assert.Contains(highest!.Value, roll.HighestRemoved.Select(r => r.Value).ToArray());

            roll.NumberOfLowestRemoved = i;
            Assert.Equal(i, roll.NumberOfLowestRemoved);
            Assert.Equal(i, roll.LowestRemoved.Length);
            Assert.Contains(lowest!.Value, roll.LowestRemoved.Select(r => r.Value).ToArray());
        }
    }
}

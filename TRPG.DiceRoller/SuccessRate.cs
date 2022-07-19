namespace TRPG.DiceRoller;

public record SuccessRate
{
    public double Value { get; init; }
    public double Percent => Math.Round(Value * 100, 2);
    public SuccessRate(double value)
    {
        Value = Math.Round(value, 4);
    }
}

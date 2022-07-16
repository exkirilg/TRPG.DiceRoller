namespace TRPG.DiceRoller.Dices.Abstract;

public abstract class Dice : IComparable<Dice>
{
    public abstract int NumberOfSides { get; }
    public int CompareTo(Dice? other) => NumberOfSides.CompareTo(other?.NumberOfSides);
}

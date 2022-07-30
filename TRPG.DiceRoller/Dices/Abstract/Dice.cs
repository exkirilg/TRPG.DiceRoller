namespace TRPG.DiceRoller.Dices.Abstract;

public abstract class Dice : IComparable<Dice>
{
    public abstract int NumberOfSides { get; }
    public int[] SidesAsIntArray
    {
        get
        {
            var resutl = new int[NumberOfSides];
            for (int i = 1; i <= NumberOfSides; i++)
                resutl[i - 1] = i;

            return resutl;
        }
    }
    public int CompareTo(Dice? other) => NumberOfSides.CompareTo(other?.NumberOfSides);

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
            return false;

        return NumberOfSides.Equals(((Dice)obj).NumberOfSides);
    }
    public override int GetHashCode()
    {
        return NumberOfSides.GetHashCode();
    }
}

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
}

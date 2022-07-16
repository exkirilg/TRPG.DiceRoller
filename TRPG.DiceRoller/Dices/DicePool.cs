using System.Collections;

namespace TRPG.DiceRoller.Dices;

public class DicePool  : IReadOnlyCollection<Dice>
{
    private readonly Dice[] _dicePool;

    public int Count => _dicePool.Length;

    public DicePool(params Dice[] dices) => _dicePool = dices;

    public IEnumerator<Dice> GetEnumerator() => (_dicePool as IEnumerable<Dice>).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public Dice this[int index] => _dicePool[index];
}

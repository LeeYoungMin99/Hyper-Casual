using System.Collections.Generic;

public class AbilityTagEnumComparer : IEqualityComparer<EAbilityTag>
{
    public bool Equals(EAbilityTag x, EAbilityTag y)
    {
        return x == y;
    }

    public int GetHashCode(EAbilityTag x)
    {
        return (int)x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumComparer : IEqualityComparer<EAbilityTag>
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

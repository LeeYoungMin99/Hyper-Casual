using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Ability
{
    public Ricochet()
    {
        Order = 1;

        _tag = EAbilityTag.Ricochet;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }
}

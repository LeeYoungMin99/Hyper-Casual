using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Ability
{
    public Boomerang()
    {
        Order = 3;

        _tag = EAbilityTag.Boomerang;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyWall : Ability
{
    public BouncyWall()
    {
        Order = 2;

        _tag = EAbilityTag.BouncyWall;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }
}

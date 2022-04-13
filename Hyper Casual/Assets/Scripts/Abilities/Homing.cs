using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Ability
{
    public Homing()
    {
        _tag = EAbilityTag.Homing;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }
}

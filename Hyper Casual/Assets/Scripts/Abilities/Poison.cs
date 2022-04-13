using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Ability
{
    public Poison()
    {
        _tag = EAbilityTag.Poison;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }
}

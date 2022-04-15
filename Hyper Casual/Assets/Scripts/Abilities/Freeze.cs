using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Ability
{
    public Freeze()
    {
        _tag = EAbilityTag.Freeze;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override void FixedUpdateAbility(Projectile projectile) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Ability
{
    public Piercing()
    {
        Order = 4;

        _tag = EAbilityTag.Piercing;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override void FixedUpdateAbility(Projectile projectile) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaze : Ability
{
    public Blaze()
    {
        _tag = EAbilityTag.Blaze;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override void FixedUpdateAbility(Projectile projectile) { }
}

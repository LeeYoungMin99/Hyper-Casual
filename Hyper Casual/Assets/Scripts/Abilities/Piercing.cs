using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Ability
{
    public Piercing()
    {
        Order = 1;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        if (LayerValue.WALL_LAYER == other.gameObject.layer || LayerValue.MAP_LAYER == other.gameObject.layer) return false;

        int bounceCount = projectile.MonsterBounceCount;
        if (0 < bounceCount && bounceCount < 3) return false;

        projectile.Damage *= 0.67f;

        return true;
    }
}

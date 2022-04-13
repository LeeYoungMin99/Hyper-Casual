using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Ability
{
    public Boomerang()
    {
        Order = 3;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        int layer = other.gameObject.layer;
        if (LayerValue.WALL_LAYER != layer && LayerValue.MAP_LAYER != layer) return true;

        if (0 < projectile.WallBounceCount && projectile.WallBounceCount > 3) return true;

        projectile.IsReturning = true;

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        if (LayerValue.WALL_LAYER == other.gameObject.layer) return false;

        Character hitCharacter = other.GetComponent<Character>();
        PoisonEffect statusEffet = hitCharacter.GetStatusEffect<PoisonEffect>();
        if (null == statusEffet)
        {
            statusEffet = new PoisonEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Init(projectile.Damage,
                         projectile.CriticalMultiplier,
                         projectile.CriticalRate);

        return false;
    }
}

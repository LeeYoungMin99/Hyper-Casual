using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaze : Ability
{
    private const float DURATION = 2f;

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        if (LayerValue.WALL_LAYER == other.gameObject.layer || LayerValue.MAP_LAYER == other.gameObject.layer) return false;

        Character hitCharacter = other.GetComponent<Character>();
        BlazeEffect statusEffet = hitCharacter.GetStatusEffect<BlazeEffect>();
        if (null == statusEffet)
        {
            statusEffet = new BlazeEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Activate(hitCharacter,
                             DURATION,
                             projectile.Damage,
                             projectile.CriticalMultiplier,
                             projectile.CriticalRate);

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Ability
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
        FreezeEffect statusEffet = hitCharacter.GetStatusEffect<FreezeEffect>();
        if (null == statusEffet)
        {
            statusEffet = new FreezeEffect();

            hitCharacter.AddStatusEffect(statusEffet);
        }

        statusEffet.Activate(hitCharacter, DURATION, 0f, 0f, 0f);

        return false;
    }
}

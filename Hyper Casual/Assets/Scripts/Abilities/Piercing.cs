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

    public override void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce)
    {
        if (LayerValue.MAP_OBJECT_LAYER == other.gameObject.layer) return;

        damage *= 0.67f;
    }
}

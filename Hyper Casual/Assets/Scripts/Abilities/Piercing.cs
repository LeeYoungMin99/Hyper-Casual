using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Ability
{
    public override void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce,
                                   ref bool isActive,
                                   ref bool isFreeze,
                                   ref bool isBlaze,
                                   ref bool isPoisonous)
    {
        if (false == IsEnabled) return;

        if (LayerValue.MAP_OBJECT_LAYER == other.gameObject.layer) return;

        if (true == isActive) return;

        isActive = true;

        damage *= 0.67f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Ability
{
    public override void InvokeAbility(Transform transform, Collider other, float criticalMultiplier, float CriticalRate, ref float damage, ref int wallBounce, ref int monsterBounce, ref bool isActive)
    {
        if (false == IsEnabled) return;

        if (6 == other.gameObject.layer) return;

        isActive = true;

        damage *= 0.67f;
    }
}

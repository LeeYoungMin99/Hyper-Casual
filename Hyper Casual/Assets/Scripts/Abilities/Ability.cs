using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public bool IsEnabled = false;

    public abstract void InvokeAbility(
        Transform transform,
        Collider other,
        float criticalMultiplier,
        float CriticalRate,
        ref float damage,
        ref int wallBounce,
        ref int monsterBounce,
        ref bool isActive);
}

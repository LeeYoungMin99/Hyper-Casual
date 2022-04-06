using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Ability
{
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
    }
}

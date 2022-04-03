using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Ability
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

        isFreeze = true;
    }
}

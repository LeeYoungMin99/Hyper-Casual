using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAbilityTag
{
    AttackDamageUp  ,
    AttackSppedUp   ,
    CriticalUp      ,
    MaxHealthUp     ,
    MultiShot       ,
    FrontArrow      ,
    DiagonalArrows  ,
    SideArrows      ,
    RearArrow       ,  
    Piercing        ,
    Ricochet        ,
    BouncyWall      ,
    WallWalker      ,
    WaterWalker
}

public abstract class Ability
{
    public bool IsEnabled = false;

    public abstract void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce,
                                   ref bool isActive,
                                   ref bool isFreeze,
                                   ref bool isBlaze,
                                   ref bool isPoisonous);
}

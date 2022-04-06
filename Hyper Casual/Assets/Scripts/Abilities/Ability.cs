using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAbilityTag
{
    AttackDamageUp,
    AttackSppedUp,
    CriticalUp,
    MaxHealthUp,
    FrontArrow,
    DiagonalArrows,
    SideArrows,
    RearArrow,
    MultiShot,
    Piercing,
    Ricochet,
    BouncyWall,
    WallWalker,
    WaterWalker
}

public abstract class Ability
{
    public int Order = 0;

    public abstract void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce);

    public abstract void ApplyAbility(Player character, Weapon weapon);
}

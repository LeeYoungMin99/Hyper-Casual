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
    Blaze,
    Freeze,
    Poison
}

public abstract class Ability
{
    public int Order = 0;

    public virtual void ApplyAbility(Player character, Weapon weapon) { }

    public virtual bool InvokeAbility(Projectile projectile, Collider other) { return false; }
}

using UnityEngine;

public enum EAbilityTag
{
    Homing,
    Boomerang,
    Piercing,
    Ricochet,
    BouncyWall,
    Blaze,
    Freeze,
    Poison,
    MultiShot,
    AttackDamageUp,
    AttackSppedUp,
    CriticalUp,
    MaxHealthUp,
    FrontArrow,
    DiagonalArrows,
    SideArrows,
    RearArrow
}

public abstract class Ability
{
    public int Order = 0;
    protected EAbilityTag _tag;

    public virtual void ApplyAbility(Player character, Weapon weapon) { }

    public virtual void InvokeAbility(Projectile projectile, Collider other)
    {
        projectile.InvokeAbility(_tag, other);
    }

    public virtual void FixedUpdateAbility(Projectile projectile)
    {
        projectile.FixedUpdateAbility(_tag);
    }
}

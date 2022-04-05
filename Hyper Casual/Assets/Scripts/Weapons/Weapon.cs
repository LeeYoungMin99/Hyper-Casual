using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    protected Ability[] _abilities;
    protected ObjectPoolingManager<Projectile> _objectPoolingManager;

    public Weapon()
    {
        Init();
    }

    public virtual void Init()
    {
        _abilities = new Ability[6];

        _abilities[0] = new BouncyWall() { IsEnabled = true };
        _abilities[1] = new Ricochet() { IsEnabled = true };
        _abilities[2] = new Piercing() { IsEnabled = true };
        _abilities[3] = new Freeze() { IsEnabled = true };
        _abilities[4] = new Blaze() { IsEnabled = true };
        _abilities[5] = new Poison() { IsEnabled = true };
    }

    public void Attack(Transform transform,
                       float damage,
                       float criticalMultiplier,
                       float criticalRate,
                       int frontFireCount,
                       int rearFireCount,
                       int SideFireCount,
                       int diagonalFireCount)
    {
        FireHelper(transform, damage, criticalMultiplier, criticalRate, transform.right, transform.transform.eulerAngles.y, frontFireCount);

        if (0 != rearFireCount)
        {
            FireHelper(transform, damage, criticalMultiplier, criticalRate, transform.right, transform.transform.eulerAngles.y + 180f, rearFireCount);
        }

        if (0 != SideFireCount)
        {
            FireHelper(transform, damage, criticalMultiplier, criticalRate, transform.forward, transform.transform.eulerAngles.y - 90f, SideFireCount);

            FireHelper(transform, damage, criticalMultiplier, criticalRate, transform.forward, transform.transform.eulerAngles.y + 90f, SideFireCount);
        }

        if (0 != diagonalFireCount)
        {
            float angle = 90f / (diagonalFireCount + 1);

            for (int i = diagonalFireCount; i > 0; --i)
            {
                Fire(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (-angle * i), transform.position);
                Fire(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (angle * i), transform.position);
            }
        }
    }

    public Vector3 CalculatePosition(Transform transform, Vector3 dir, int maxCount, int curCount)
    {
        Vector3 startPosition = transform.position + dir * (0.1f * (maxCount - 1));

        return startPosition + dir * (-0.2f * curCount);
    }

    public void Fire(float damage, float criticalMultiplier, float criticalRate, float angle, Vector3 position)
    {
        Projectile projectile = _objectPoolingManager.GetObject();

        projectile.Damage = damage;
        projectile.CriticalMultiplier = criticalMultiplier;
        projectile.CriticalRate = criticalRate;
        projectile.Abilities = _abilities;
        projectile.gameObject.transform.position = position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        projectile.gameObject.SetActive(true);
    }

    public void FireHelper(Transform transform, float damage, float criticalMultiplier, float criticalRate, Vector3 dir, float angle, int maxFireCount)
    {
        for (int i = 0; i < maxFireCount; ++i)
        {
            Vector3 position = CalculatePosition(transform, dir, maxFireCount, i);

            Fire(damage, criticalMultiplier, criticalRate, angle, position);
        }
    }
}

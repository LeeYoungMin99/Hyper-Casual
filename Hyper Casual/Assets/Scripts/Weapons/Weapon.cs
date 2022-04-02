using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public Transform Owner;
    public ObjectPoolingManager ObjectPoolingManager;

    protected Ability[] _abilities;

    public Weapon(Transform owner)
    {
        Owner = owner;
    }

    public abstract void Init();

    public void Attack(float damage,
                       float criticalMultiplier,
                       float criticalRate,
                       int frontFireCount,
                       int rearFireCount,
                       int SideFireCount,
                       int diagonalFireCount)
    {
        FireHelper(damage, criticalMultiplier, criticalRate, Owner.right, Owner.transform.eulerAngles.y, frontFireCount);

        if (0 != rearFireCount)
        {
            FireHelper(damage, criticalMultiplier, criticalRate, Owner.right, Owner.transform.eulerAngles.y + 180f, rearFireCount);
        }

        if (0 != SideFireCount)
        {
            FireHelper(damage, criticalMultiplier, criticalRate, Owner.forward, Owner.transform.eulerAngles.y - 90f, SideFireCount);

            FireHelper(damage, criticalMultiplier, criticalRate, Owner.forward, Owner.transform.eulerAngles.y + 90f, SideFireCount);
        }

        if (0 != diagonalFireCount)
        {
            float angle = 90f / (diagonalFireCount + 1);

            for (int i = diagonalFireCount; i > 0; --i)
            {
                Fire(damage, criticalMultiplier, criticalRate, Owner.transform.eulerAngles.y + (-angle * i), Owner.position);
                Fire(damage, criticalMultiplier, criticalRate, Owner.transform.eulerAngles.y + (angle * i), Owner.position);
            }
        }
    }

    public Vector3 CalculatePosition(Vector3 dir, int maxCount, int curCount)
    {
        Vector3 startPosition = Owner.position + dir * (0.1f * (maxCount - 1));

        return startPosition + dir * (-0.2f * curCount);
    }

    public void Fire(float damage, float criticalMultiplier, float criticalRate, float angle, Vector3 position)
    {
        Projectile projectile = ObjectPoolingManager.GetObject();

        projectile.Damage = damage;
        projectile.CriticalMultiplier = criticalMultiplier;
        projectile.CriticalRate = criticalRate;
        projectile.gameObject.transform.position = position;
        projectile.gameObject.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        projectile.gameObject.SetActive(true);
    }

    public void FireHelper(float damage, float criticalMultiplier, float criticalRate, Vector3 dir, float angle, int maxFireCount)
    {
        for (int i = 0; i < maxFireCount; ++i)
        {
            Vector3 position = CalculatePosition(dir, maxFireCount, i);

            Fire(damage, criticalMultiplier, criticalRate, angle, position);
        }
    }
}

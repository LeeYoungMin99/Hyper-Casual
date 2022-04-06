using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public class AddComparer : IComparer<Ability>
    {
        public int Compare(Ability x, Ability y)
        {
            if (x.Order < y.Order) return -1;

            if (x.Order > y.Order) return 1;

            return 0;
        }
    }

    protected List<Ability> _abilities = new List<Ability>();
    protected AddComparer _addComparer = new AddComparer();
    protected ObjectPoolingManager<Projectile> _objectPoolingManager;

    public Weapon()
    {
        Init();
    }

    public abstract void Init();

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

    public void AddAbility(Ability ability)
    {
        _abilities.Add(ability);

        _abilities.Sort(_addComparer.Compare);
    }

    private Vector3 CalculateProjectilePosition(Transform transform, Vector3 dir, int maxCount, int curCount)
    {
        Vector3 startPosition = transform.position + dir * (0.1f * (maxCount - 1));

        return startPosition + dir * (-0.2f * curCount);
    }

    private void Fire(float damage, float criticalMultiplier, float criticalRate, float angle, Vector3 position)
    {
        Projectile projectile = _objectPoolingManager.GetObject();

        projectile.Init(damage, criticalMultiplier, criticalRate, _abilities, position, angle);
    }

    private void FireHelper(Transform transform, float damage, float criticalMultiplier, float criticalRate, Vector3 dir, float angle, int maxFireCount)
    {
        for (int i = 0; i < maxFireCount; ++i)
        {
            Vector3 position = CalculateProjectilePosition(transform, dir, maxFireCount, i);

            Fire(damage, criticalMultiplier, criticalRate, angle, position);
        }
    }
}

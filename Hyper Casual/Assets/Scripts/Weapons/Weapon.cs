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

    protected Character _owner;
    protected List<Ability> _abilities = new List<Ability>();
    protected AddComparer _addComparer = new AddComparer();
    protected ObjectPoolingManager<Projectile> _objectPoolingManager;

    public Weapon(Character owner)
    {
        _owner = owner;

        Init();
    }

    public abstract void Init();

    public void Attack(Transform transform,
                       float damage,
                       float criticalMultiplier = 2f,
                       float criticalRate = 0f,
                       int frontFireCount = 0,
                       int rearFireCount = 0,
                       int sideFireCount = 0,
                       int frontDiagonalFireCount = 0,
                       int rearDiagonalFireCount = 0)
    {
        AttackHelper(transform, damage, criticalMultiplier, criticalRate, transform.right, transform.transform.eulerAngles.y, frontFireCount);

        if (0 != rearFireCount)
        {
            AttackHelper(transform, damage, criticalMultiplier, criticalRate, transform.right, transform.transform.eulerAngles.y + 180f, rearFireCount);
        }

        if (0 != sideFireCount)
        {
            AttackHelper(transform, damage, criticalMultiplier, criticalRate, transform.forward, transform.transform.eulerAngles.y - 90f, sideFireCount);

            AttackHelper(transform, damage, criticalMultiplier, criticalRate, transform.forward, transform.transform.eulerAngles.y + 90f, sideFireCount);
        }

        if (0 != frontDiagonalFireCount)
        {
            float angle = 90f / (frontDiagonalFireCount + 1);

            for (int i = frontDiagonalFireCount; i > 0; --i)
            {
                InitProjectile(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (-angle * i), transform.position);
                InitProjectile(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (angle * i), transform.position);
            }
        }

        if (0 != rearDiagonalFireCount)
        {
            float angle = 90f / (frontDiagonalFireCount + 1) + 180f;

            for (int i = frontDiagonalFireCount; i > 0; --i)
            {
                InitProjectile(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (-angle * i), transform.position);
                InitProjectile(damage, criticalMultiplier, criticalRate, transform.eulerAngles.y + (angle * i), transform.position);
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

    private void InitProjectile(float damage, float criticalMultiplier, float criticalRate, float angle, Vector3 position)
    {
        Projectile projectile = _objectPoolingManager.GetObject();

        projectile.Init(damage, criticalMultiplier, criticalRate, _abilities, position, angle);
    }

    private void AttackHelper(Transform transform, float damage, float criticalMultiplier, float criticalRate, Vector3 dir, float angle, int maxFireCount)
    {
        for (int i = 0; i < maxFireCount; ++i)
        {
            Vector3 position = CalculateProjectilePosition(transform, dir, maxFireCount, i);

            InitProjectile(damage, criticalMultiplier, criticalRate, angle, position);
        }
    }
}

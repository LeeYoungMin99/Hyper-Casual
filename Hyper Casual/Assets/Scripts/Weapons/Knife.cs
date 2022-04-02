using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public Knife(Transform owner)
        : base(owner)
    {
        Init();
    }

    public override void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Projectile/Knife");

        _abilities = new Ability[3];

        _abilities[0] = new BouncyWall() { IsEnabled = false };
        _abilities[1] = new Piercing() { IsEnabled = true };
        _abilities[2] = new Ricochet() { IsEnabled = true };

        ObjectPoolingManager = new ObjectPoolingManager(prefab, _abilities);
    }
}

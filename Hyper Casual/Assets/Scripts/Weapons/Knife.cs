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

        _abilities = new Ability[0];

        ObjectPoolingManager = new ObjectPoolingManager(prefab, _abilities);

    }
}

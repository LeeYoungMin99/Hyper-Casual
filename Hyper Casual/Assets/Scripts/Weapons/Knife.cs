using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public override void Init()
    {
        base.Init();

        GameObject prefab = Resources.Load<GameObject>("Projectile/Knife");

        _objectPoolingManager = new ObjectPoolingManager(prefab);
    }
}

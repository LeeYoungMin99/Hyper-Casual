using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    public Staff(Character owner) : base(owner) { }

    public override void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Projectile/Fire Ball");

        prefab = GameObject.Instantiate(prefab, _owner.transform);

        prefab.layer = _layer;

        _objectPoolingManager = new ObjectPoolingManager<Projectile>(prefab);
    }
}

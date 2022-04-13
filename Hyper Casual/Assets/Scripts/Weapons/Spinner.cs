using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Weapon
{
    public Spinner(Character owner) : base(owner) { }

    public override void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Projectile/Spinner");

        prefab = GameObject.Instantiate(prefab, _owner.transform);

        prefab.layer = _layer;

        _objectPoolingManager = new ObjectPoolingManager<Projectile>(prefab);

        AddAbility(new Boomerang());
    }
}

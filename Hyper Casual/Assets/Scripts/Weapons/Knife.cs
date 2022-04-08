using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public Knife(Character owner) : base(owner) { }

    public override void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Projectile/Knife");

        prefab = GameObject.Instantiate(prefab, _owner.transform);

        if (true == (_owner.GetType() == typeof(Player)))
        {
            prefab.layer = LayerValue.FRIENDLY_PROJECTILE;
        }
        else
        {
            prefab.layer = LayerValue.ENEMY_PROJECTILE;
        }

        _objectPoolingManager = new ObjectPoolingManager<Projectile>(prefab);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    public override void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Projectile/Knife");

        _abilities = new Ability[6];

        _abilities[0] = new BouncyWall()    { IsEnabled = true };
        _abilities[1] = new Ricochet()      { IsEnabled = true };
        _abilities[2] = new Piercing()      { IsEnabled = true };
        _abilities[3] = new Freeze()        { IsEnabled = true };
        _abilities[4] = new Blaze()         { IsEnabled = true };
        _abilities[5] = new Poison()        { IsEnabled = true };

        ObjectPoolingManager = new ObjectPoolingManager(prefab, _abilities);
    }
}

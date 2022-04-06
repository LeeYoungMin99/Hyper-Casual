using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Ability
{
    private Collider[] _colliders = new Collider[8];

    private const int MAX_BOUNCE_COUNT = 3;

    public Ricochet()
    {
        Order = 2;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce)
    {
        if (LayerValue.MAP_OBJECT_LAYER == other.gameObject.layer) return;

        if (MAX_BOUNCE_COUNT <= monsterBounce) return;

        ++monsterBounce;

        damage *= 0.7f;

        int count = Physics.OverlapSphereNonAlloc(other.transform.position, 10f, _colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        if (count == 0) return;

        transform.gameObject.SetActive(true);

        transform.position += transform.forward * (Time.deltaTime * 20f);

        for (int i = 0; i < count; ++i)
        {
            if (other == _colliders[i]) continue;

            Vector3 targetDir = (_colliders[i].transform.position - transform.position).normalized;

            float angle = Utils.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Ability
{
    private Collider[] _colliders = new Collider[8];
    private RaycastHit _hit;
    private Ray _ray = new Ray();

    private const int MAX_BOUNCE_COUNT = 3;

    public override void InvokeAbility(Transform transform,
                                       Collider other,
                                       float criticalMultiplier,
                                       float CriticalRate,
                                   ref float damage,
                                   ref int wallBounce,
                                   ref int monsterBounce,
                                   ref bool isActive,
                                   ref bool isFreeze,
                                   ref bool isBlaze,
                                   ref bool isPoisonous)
    {
        if (false == IsEnabled) return;

        if (LayerValue.MAP_OBJECT_LAYER == other.gameObject.layer) return;

        if (MAX_BOUNCE_COUNT <= monsterBounce) return;

        isActive = true;

        ++monsterBounce;

        damage *= 0.7f;

        int count = Physics.OverlapSphereNonAlloc(other.transform.position, 10f, _colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        if (count == 0) return;

        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        other.Raycast(_ray, out _hit, 10f);

        transform.position = _hit.point;

        for (int i = 0; i < count; ++i)
        {
            if (other == _colliders[i]) continue;

            Vector3 targetDir = (_colliders[i].transform.position - transform.position).normalized;

            float angle = Utils.Instance.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Ability
{
    private Collider[] _colliders = new Collider[8];
    private LayerMask _layerMask = (1 << 13) + (1 << 14) + (1 << 15) + (1 << 16);
    private RaycastHit _hit;
    private Ray _ray = new Ray();

    private const int MAX_BOUNCE_COUNT = 3;

    public override void InvokeAbility(Transform transform, Collider other, float criticalMultiplier, float CriticalRate, ref float damage, ref int wallBounce, ref int monsterBounce, ref bool isActive)
    {
        if (false == IsEnabled) return;

        if (6 == other.gameObject.layer) return;

        if (MAX_BOUNCE_COUNT <= monsterBounce) return;

        isActive = true;

        ++monsterBounce;

        damage *= 0.7f;

        int count = Physics.OverlapSphereNonAlloc(other.transform.position, 10f, _colliders, _layerMask);

        if (count == 0) return;

        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        for (int i = 0; i < count; ++i)
        {
            if (other == _colliders[i]) continue;

            other.Raycast(_ray, out _hit, 10f);

            transform.position = _hit.point;

            Vector3 targetDir = (_colliders[i].transform.position - transform.position).normalized;

            float angle = Utils.Instance.CalculateAngle(targetDir, transform.forward);

            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);

            break;
        }
    }
}

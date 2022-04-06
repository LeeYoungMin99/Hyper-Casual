using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyWall : Ability
{
    private RaycastHit _hit;
    private Ray _ray = new Ray();
    private const int MAX_BOUNCE_COUNT = 3;

    public BouncyWall()
    {
        Order = 3;
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
        if (LayerValue.MAP_OBJECT_LAYER != other.gameObject.layer) return;

        if (MAX_BOUNCE_COUNT <= wallBounce) return;

        transform.gameObject.SetActive(true);

        ++wallBounce;

        damage *= 0.5f;

        _ray.origin = transform.position;
        _ray.direction = transform.forward;

        other.Raycast(_ray, out _hit, 30f);

        Vector3 reflect = Vector3.Reflect(transform.forward, _hit.normal).normalized;

        float angle = Utils.CalculateAngle(reflect, transform.forward);

        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + angle, 0f);
    }
}

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

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        if (LayerValue.WALL_LAYER != other.gameObject.layer && LayerValue.MAP_LAYER != other.gameObject.layer) return false;

        if (MAX_BOUNCE_COUNT <= projectile.WallBounceCount) return false;

        ++projectile.WallBounceCount;

        projectile.Damage *= 0.5f;

        _ray.origin = projectile.transform.position;
        _ray.direction = projectile.transform.forward;

        other.Raycast(_ray, out _hit, 10f);

        Vector3 reflect = Vector3.Reflect(projectile.transform.forward, _hit.normal).normalized;

        float angle = Utils.CalculateAngle(reflect, projectile.transform.forward);
        
        projectile.transform.rotation = Quaternion.Euler(0f, projectile.transform.eulerAngles.y + angle, 0f);

        return true;
    }
}

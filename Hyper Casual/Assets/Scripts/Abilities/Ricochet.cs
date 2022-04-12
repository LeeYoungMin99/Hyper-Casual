using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Ability
{
    private const int MAX_BOUNCE_COUNT = 3;

    public Ricochet()
    {
        Order = 1;
    }

    public override void ApplyAbility(Player character, Weapon weapon)
    {
        weapon.AddAbility(this);
    }

    public override bool InvokeAbility(Projectile projectile, Collider other)
    {
        if (LayerValue.WALL_LAYER == other.gameObject.layer || LayerValue.MAP_LAYER == other.gameObject.layer) return false;

        if (MAX_BOUNCE_COUNT <= projectile.MonsterBounceCount) return false;

        ++projectile.MonsterBounceCount;

        projectile.Damage *= 0.7f;

        int count = Physics.OverlapSphereNonAlloc(other.transform.position, 10f, Utils.Colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        if (count == 0) return false;

        Transform projectileTransform = projectile.transform;

        projectileTransform.position += projectileTransform.forward * (Time.deltaTime * 20f);

        for (int i = 0; i < count; ++i)
        {
            if (other == Utils.Colliders[i]) continue;

            Vector3 targetDir = (Utils.Colliders[i].transform.position - projectileTransform.position).normalized;

            float angle = Utils.CalculateAngle(targetDir, projectileTransform.forward);

            projectileTransform.rotation = Quaternion.Euler(0f, projectileTransform.eulerAngles.y + angle, 0f);

            break;
        }

        return true;
    }
}

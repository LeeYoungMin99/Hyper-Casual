using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    [SerializeField] private float _rotateSpeed = 1f;
    private float _homingElapsedTime;
    private int _targetLayerMask;

    private const float HOMING_TIME = 0.2f;

    protected override void Awake()
    {
        base.Awake();

        if (LayerValue.FRIENDLY_PROJECTILE == gameObject.layer)
        {
            _targetLayerMask = LayerValue.ALL_ENEMY_LAYER_MASK;
        }
        else
        {
            _targetLayerMask = LayerValue.ALL_PLAYER_LAYER_MASK;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _homingElapsedTime = 0f;
    }

    protected override void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(_rigidbody.position, 3f, Utils.Colliders, _targetLayerMask);

        if (0 != count && HOMING_TIME > _homingElapsedTime)
        {
            Collider target = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; ++i)
            {
                float distance = Vector3.Distance(Utils.Colliders[i].transform.position, transform.position);

                if (minDistance <= distance) continue;

                Vector3 dir = (Utils.Colliders[i].transform.position - transform.position).normalized;

                float dot = Mathf.Clamp(Vector3.Dot(transform.forward, dir), -1f, 1f);

                if (0f >= dot) continue;

                minDistance = distance;
                target = Utils.Colliders[i];
            }

            if (target != null)
            {
                _homingElapsedTime += Time.deltaTime;

                Vector3 dir = target.attachedRigidbody.position - _rigidbody.position;
                Quaternion dirQuat = Quaternion.LookRotation(dir);
                Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, Time.deltaTime * _rotateSpeed);

                _rigidbody.MoveRotation(moveQuat);
            }
        }

        base.FixedUpdate();
    }
}

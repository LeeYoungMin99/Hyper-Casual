using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAndBounceWall : State
{
    private Rigidbody _rigidbody;
    private Ray _ray = new Ray();
    private RaycastHit _hit;
    private float _moveSpeed;

    public RunAndBounceWall(StateMachine stateMachine, Character owner, float moveSpeed) : base(stateMachine, owner)
    {
        _rigidbody = owner.GetComponent<Rigidbody>();
        _moveSpeed = moveSpeed;
    }

    public override void FixedUpdate()
    {
        Vector3 newVelocity = _owner.transform.forward * (Time.deltaTime * _moveSpeed);

        _rigidbody.velocity = newVelocity;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (LayerValue.WALL_LAYER != other.gameObject.layer && LayerValue.MAP_LAYER != other.gameObject.layer) return;

        _ray.origin = _owner.transform.position;
        _ray.direction = _owner.transform.forward;

        other.Raycast(_ray, out _hit, 10f);

        Vector3 reflect = Vector3.Reflect(_owner.transform.forward, _hit.normal).normalized;

        float angle = Utils.CalculateAngle(reflect, _owner.transform.forward);

        _owner.transform.rotation = Quaternion.Euler(0f, _owner.transform.eulerAngles.y + angle, 0f);
    }
}

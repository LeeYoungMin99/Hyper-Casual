using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : State
{
    private Transform _target;
    private Rigidbody _rigidbody;
    private float _rotateSpeed;

    public LookAtTarget(StateMachine stateMachine, Character owner, float rotateSpeed) : base(stateMachine, owner)
    {
        _rotateSpeed = rotateSpeed;
        _rigidbody = owner.GetComponent<Rigidbody>();

        Physics.OverlapSphereNonAlloc(_owner.transform.position, 100f, Utils.Colliders, LayerValue.ALL_PLAYER_LAYER_MASK);
        _target = Utils.Colliders[0].transform;
    }

    public override void FixedUpdate()
    {
        Quaternion dirQuat = Quaternion.LookRotation(_target.position - _owner.transform.position);
        Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, _rotateSpeed * Time.deltaTime);

        _rigidbody.MoveRotation(moveQuat);

        _transitionParameter.Time += Time.deltaTime;
    }
}

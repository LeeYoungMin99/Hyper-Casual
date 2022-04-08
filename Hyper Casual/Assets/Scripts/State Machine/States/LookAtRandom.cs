using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRandom : State
{
    private float _randomAngle;
    private float _rotateSpeed;
    private Rigidbody _rigidbody;

    public LookAtRandom(StateMachine stateMachine, Character owner, float rotateSpeed) : base(stateMachine, owner)
    {
        _rotateSpeed = rotateSpeed;
        _rigidbody = owner.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        base.Enter();

        _randomAngle = Random.Range(0f, 360f);
    }

    public override void FixedUpdate()
    {
        Quaternion dirQuat = Quaternion.Euler(0f, _randomAngle, 0f);
        Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, _rotateSpeed * Time.deltaTime);
        _rigidbody.MoveRotation(moveQuat);

        _transitionParameter.Time += Time.deltaTime;
    }
}

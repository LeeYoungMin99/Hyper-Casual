using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _moveSpeed;

    public Dash(StateMachine stateMachine, Character owner, float moveSpeed) : base(stateMachine, owner)
    {
        _moveSpeed = moveSpeed;
        _rigidbody = owner.GetComponent<Rigidbody>();
        _animator = owner.GetComponent<Animator>();
    }

    public override void Enter()
    {
        base.Enter();

        _animator.SetBool(AnimationID.DASH, true);
    }

    public override void FixedUpdate()
    {
        Vector3 newVelocity = _owner.transform.forward * (_moveSpeed * Time.deltaTime);
        _rigidbody.velocity = newVelocity;

        _transitionParameter.Time += Time.deltaTime;
    }

    public override void Exit()
    {
        _rigidbody.velocity = Utils.ZERO_VECTOR2;
        _animator.SetBool(AnimationID.DASH, false);
    }
}

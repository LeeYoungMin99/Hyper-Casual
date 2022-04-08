using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : State
{
    private Weapon _weapon;
    private Animator _animator;
    private float _damage;
    private float _delay;
    private int _frontShotCount;
    private int _rearShotCount;
    private int _sideShotCount;
    private int _frontDiagonalShotCount;
    private int _rearDiagonalShotCount;

    private bool _isShot = false;

    public Shot(StateMachine stateMachine,
                Character owner,
                Weapon weapon,
                float damage,
                float delay,
                int frontShotCount,
                int rearShotCount,
                int sideShotCount,
                int frontDiagonalShotCount,
                int rearDiagonalShotCount) : base(stateMachine, owner)
    {
        _animator = owner.GetComponent<Animator>();

        _weapon = weapon;
        _damage = damage;
        _delay = delay;

        _frontShotCount = frontShotCount;
        _rearShotCount = rearShotCount;
        _sideShotCount = sideShotCount;
        _frontDiagonalShotCount = frontDiagonalShotCount;
        _rearDiagonalShotCount = rearDiagonalShotCount;
    }

    public override void Enter()
    {
        base.Enter();

        _animator.SetTrigger(AnimationID.SHOT);

        _isShot = false;
    }

    public override void FixedUpdate()
    {
        if (false == _isShot && _delay <= _transitionParameter.Time)
        {
            _weapon.Attack(_owner.transform, _damage, 0f, 0f,
                           _frontShotCount,
                           _rearShotCount,
                           _sideShotCount,
                           _frontDiagonalShotCount,
                           _rearDiagonalShotCount);

            _isShot = true;
        }

        _transitionParameter.Time += Time.deltaTime;
    }
}

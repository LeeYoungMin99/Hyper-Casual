using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBoss : Monster
{
    protected override void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        HealthBarManager.Instance.CreateHealthBar(this, EHealthBarType.Boss);
        InvokeChangeHealthEvent();

        LookAtRandom lookAtRandom = new LookAtRandom(_stateMachine, this, 10f);
        Dash dash = new Dash(_stateMachine, this, 200f);
        LookAtTarget lookAtTarget = new LookAtTarget(_stateMachine, this, 10f);
        Shot shot = new Shot(_stateMachine, this, new HomingStaff(this), _attackDamage, 0.3f, 1, 1, 1, 3, 3);

        lookAtRandom.AddTransition(new TimeTransition(dash, 0.5f));

        dash.AddTransition(new TimeTransition(lookAtTarget, 1f));

        lookAtTarget.AddTransition(new TimeTransition(shot, 1f));

        shot.AddTransition(new TimeTransition(lookAtRandom, 1f));

        _stateMachine.ChangeState(lookAtRandom);
    }
}

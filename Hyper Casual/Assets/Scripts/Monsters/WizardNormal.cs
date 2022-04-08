using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardNormal : Monster
{
    protected override void Awake()
    {
        base.Awake();

        Aiming aiming = new Aiming(_stateMachine, this);
        Shot shot = new Shot(_stateMachine, this, new Staff(this), _attackDamage, 0f, 1, 0, 0, 0, 0);
        Chase chase = new Chase(_stateMachine, this, 100f, 10f);

        aiming.AddTransition(new TimeTransition(shot, 2f));

        shot.AddTransition(new TimeTransition(chase, 1f));

        chase.AddTransition(new DistanceTransition(aiming, 2f));
        chase.AddTransition(new TimeTransition(aiming, 2f));

        _stateMachine.ChangeState(aiming);
    }
}

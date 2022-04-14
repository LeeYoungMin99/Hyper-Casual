using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condor : Monster
{
    protected override void Awake()
    {
        _healthBarType = EHealthBarType.Monster;

        base.Awake();

        LookAtTarget lookAtTarget = new LookAtTarget(_stateMachine, this, 10f);
        Dash dash = new Dash(_stateMachine, this, 500f);

        lookAtTarget.AddTransition(new TimeTransition(dash, 2f));
        dash.AddTransition(new TimeTransition(lookAtTarget, 1f));

        _stateMachine.ChangeState(lookAtTarget);
    }
}

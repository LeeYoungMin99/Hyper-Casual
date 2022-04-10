using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Monster
{
    protected override void Awake()
    {
        base.Awake();

        LookAtRandom lookAtRandom = new LookAtRandom(_stateMachine, this, 10f);
        Dash dash = new Dash(_stateMachine, this, 500f);

        lookAtRandom.AddTransition(new TimeTransition(dash, 0.5f));
        dash.AddTransition(new TimeTransition(lookAtRandom, 1f));

        _stateMachine.ChangeState(lookAtRandom);
    }
}

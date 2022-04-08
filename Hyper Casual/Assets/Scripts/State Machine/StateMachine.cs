using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State _curState;

    public void FixedUpdate()
    {
        _curState.FixedUpdate();
    }

    public void Update()
    {
        _curState.Update();
    }

    public void LateUpdate()
    {
        _curState.LateUpdate();
    }

    public void OnTriggerEnter(Collider other)
    {
        _curState.OnTriggerEnter(other);
    }

    public void ChangeState(State state)
    {
        if (null != _curState)
        {
            _curState.Exit();
        }

        _curState = state;

        _curState.Enter();
    }
}

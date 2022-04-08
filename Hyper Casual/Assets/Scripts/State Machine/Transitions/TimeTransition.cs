using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTransition : Transition
{
    private float _time;

    public TimeTransition(State connectedState, float time) : base(connectedState)
    {
        _time = time;
    }

    public override bool Check(TransitionParameter parameter)
    {
        if (_time > parameter.Time) return false;

        parameter.Time = 0f;

        return true;
    }
}

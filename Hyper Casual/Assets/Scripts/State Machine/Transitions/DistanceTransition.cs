using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTransition : Transition
{
    float _distance;

    public DistanceTransition(State connectedState, float distance) : base(connectedState)
    {
        _distance = distance;
    }

    public override bool Check(TransitionParameter parameter)
    {
        if (_distance < parameter.Distance) return false;

        parameter.Distance = float.MaxValue;

        return true;
    }
}

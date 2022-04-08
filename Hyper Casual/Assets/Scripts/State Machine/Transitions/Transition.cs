using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition
{
    public State ConnectedState;

    protected Transition(State connectedState)
    {
        ConnectedState = connectedState;
    }

    public abstract bool Check(TransitionParameter parameter);
}

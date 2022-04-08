using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Character _owner;
    protected StateMachine _stateMachine;
    protected List<Transition> _transitions = new List<Transition>();
    protected TransitionParameter _transitionParameter = new TransitionParameter();

    public State(StateMachine stateMachine, Character owner)
    {
        _owner = owner;

        _stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        InitTransitionParameter();
    }

    public virtual void Exit() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void LateUpdate()
    {
        int count = _transitions.Count;
        for (int i = 0; i < count; ++i)
        {
            if (true == _transitions[i].Check(_transitionParameter))
            {
                Debug.Log(_transitions[i]);
                Debug.Log(_transitions[i].ConnectedState);
                _stateMachine.ChangeState(_transitions[i].ConnectedState);

                return;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other) { }

    public void AddTransition(Transition transition)
    {
        _transitions.Add(transition);
    }

    protected void InitTransitionParameter()
    {
        _transitionParameter.Time = 0f;
        _transitionParameter.Distance = float.MaxValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBody : State
{
    public SplitBody(StateMachine stateMachine, Character owner) : base(stateMachine, owner)
    {
        owner.HealthChangeEvent -= Split;
        owner.HealthChangeEvent += Split;
    }

    private void Split(object sender, HealthChangeEventArgs args)
    {
        if (0f < args.CurHealth) return;

        int count = _owner.transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            Transform child = _owner.transform.GetChild(i);
            if (false == child.gameObject.activeSelf)
            {
                child.SetParent(_owner.transform.parent);
                child.gameObject.SetActive(true);

                --i;
                --count;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Monster
{
    protected override void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        HealthBarManager.Instance.CreateHealthBar(this, EHealthBarType.Boss);
        InvokeChangeHealthEvent();

        None none = new None(_stateMachine, this);

        _stateMachine.ChangeState(none);
    }
}

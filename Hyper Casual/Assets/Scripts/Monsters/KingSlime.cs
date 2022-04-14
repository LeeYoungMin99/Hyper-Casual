using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Monster
{
    protected override void Awake()
    {
        _healthBarType = EHealthBarType.Boss;

        base.Awake();

        None none = new None(_stateMachine, this);

        _stateMachine.ChangeState(none);
    }
}

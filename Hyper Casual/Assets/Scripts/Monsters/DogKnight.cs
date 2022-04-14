using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogKnight : Monster
{
    [SerializeField] Collider _slashCollider;

    private const float ANIMATION_TIME = 0.833f;

    protected override void Awake()
    {
        _healthBarType = EHealthBarType.Monster;

        base.Awake();

        Chase chase = new Chase(_stateMachine, this, 200f, 10f);
        Slash slash = new Slash(_stateMachine, this, _slashCollider, _attackDamage * 2f, 0.25f);

        chase.AddTransition(new DistanceTransition(slash, 2f));
        slash.AddTransition(new TimeTransition(chase, ANIMATION_TIME));

        _stateMachine.ChangeState(chase);
    }
}

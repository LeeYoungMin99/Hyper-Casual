using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : State
{
    private Animator _animator;
    private Collider _collider;
    private float _damage;
    private float _delay;

    public Slash(StateMachine stateMachine, Character owner, Collider collider, float damage, float delay) : base(stateMachine, owner)
    {
        _animator = owner.GetComponent<Animator>();
        _collider = collider;
        _damage = damage;
        _delay = delay;
    }

    public override void Enter()
    {
        base.Enter();

        _animator.SetTrigger(AnimationID.SLASH);
    }

    public override void Exit()
    {
        _collider.enabled = false;
    }

    public override void FixedUpdate()
    {
        _transitionParameter.Time += Time.deltaTime;

        if (_transitionParameter.Time >= _delay)
        {
            _collider.enabled = true;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        int count = LayerValue.PLAYER_LAYERS.Length;
        int layer = other.gameObject.layer;
        for (int i = 0; i < count; ++i)
        {
            if (layer == LayerValue.PLAYER_LAYERS[i])
            {
                other.GetComponent<IDamageable>().TakeDamage(_damage);
            }
        }
    }
}

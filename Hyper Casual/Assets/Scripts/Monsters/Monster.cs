using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    [SerializeField] protected float _experience = 200f;

    protected StateMachine _stateMachine = new StateMachine();
    protected EHealthBarType _healthBarType;

    public event EventHandler<EventArgs> DeathEvent;

    protected override void Awake()
    {
        base.Awake();

        HealthBarManager.Instance.CreateHealthBar(this, _healthBarType);
        InvokeChangeHealthEvent();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (LayerValue.MAP_LAYER == layer)
        {
            _rigidbody.velocity = Utils.ZERO_VECTOR3;
        }

        int count = LayerValue.PLAYER_LAYERS.Length;
        for (int i = 0; i < count; ++i)
        {
            if (layer == LayerValue.PLAYER_LAYERS[i])
            {
                collision.gameObject.GetComponent<IDamageable>().TakeDamage(_attackDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _stateMachine.OnTriggerEnter(other);
    }

    protected override void FixedUpdateAct()
    {
        _stateMachine.FixedUpdate();
    }

    protected override void UpdateAct()
    {
        _stateMachine.Update();
    }

    protected override void LateUpdateAct()
    {
        _stateMachine.LateUpdate();
    }

    protected override void Death()
    {
        if (true == _isDead) return;

        base.Death();

        Vector3 position = new Vector3(transform.position.x, 0f, transform.position.z);
        ExperienceManager.Instance.CreateExperience(_experience, position);

        DeathEvent?.Invoke(this, EventArgs.Empty);
        DeathEvent = null;
    }
}

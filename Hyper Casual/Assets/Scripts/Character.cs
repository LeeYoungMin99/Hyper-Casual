using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public float MoveSpeed;

    public event EventHandler<HealthChangeEventArgs> HealthChangeEvent;

    protected Rigidbody _rigidbody;
    protected Animator _animator;
    protected float _attackDamage;
    protected float _maxHealth;
    protected float _curHealth;

    private HealthChangeEventArgs _healthChangeEventArgs = new HealthChangeEventArgs();

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _rigidbody.detectCollisions = true;
    }

    public void TakeDamage(float damage,
                           float criticalMultiplier,
                           float criticalRate)
    {
        TakeDamageHelper(damage, criticalMultiplier, criticalRate);
    }

    public void TakeDamageHelper(float damage, float criticalMultiplier, float criticalRate)
    {
        bool isCritical = false;

        if (true == CalculateCritical(criticalRate))
        {
            damage *= criticalMultiplier;
            isCritical = true;
            CameraShaker.Instance.ShakeCamera(0.5f, 0.5f);
        }

        DamageTextManager.Instance.MarkDamageText(this, damage, isCritical);
        _curHealth -= damage;

        InvokeChangeHealthEvent();

        if (0f < _curHealth) return;

        Death();
    }

    protected void InvokeChangeHealthEvent()
    {
        _healthChangeEventArgs.MaxHealth = _maxHealth;
        _healthChangeEventArgs.CurHealth = _curHealth;
        HealthChangeEvent?.Invoke(this, _healthChangeEventArgs);
    }

    protected virtual void Death()
    {
        _rigidbody.detectCollisions = false;

        StopAllCoroutines();

        HealthChangeEvent = null;
    }

    private bool CalculateCritical(float criticalRate)
    {
        if (0f >= criticalRate) return false;

        if (UnityEngine.Random.Range(0f, 100f) >= criticalRate) return false;

        return true;
    }
}

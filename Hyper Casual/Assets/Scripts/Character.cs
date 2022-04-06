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
    protected float _attackDamage = 0.001f;
    protected float _maxHealth = 100;
    protected float _curHealth = 100;

    private bool _canAct;

    private List<StatusEffect> _statusEffects = new List<StatusEffect>();

    private HealthChangeEventArgs _healthChangeEventArgs = new HealthChangeEventArgs();

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        AddStatusEffect(new FreezeEffect());
    }

    private void OnEnable()
    {
        _rigidbody.detectCollisions = true;
    }

    private void FixedUpdate()
    {
        _canAct = true;

        int count = _statusEffects.Count;
        for (int i = 0; i < count; ++i)
        {
            if (false == _statusEffects[i].Update(this))
            {
                _canAct = false;
            }
        }

        //_animator.SetBool(AnimationID.CAN_ACT, _canAct);
        if (false == _canAct) return;

        FixedUpdateAct();
    }

    private void Update()
    {
        if (false == _canAct) return;

        UpdateAct();
    }

    public void TakeDamage(float damage,
                           float criticalMultiplier,
                           float criticalRate)
    {
        TakeDamageHelper(damage, criticalMultiplier, criticalRate);
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        _statusEffects.Add(statusEffect);
    }

    public T GetStatusEffect<T>() where T : StatusEffect
    {
        int count = _statusEffects.Count;
        for (int i = 0; i < count; ++i)
        {
            if (false == (_statusEffects[i].GetType() == typeof(T))) continue;

            return (T)_statusEffects[i];
        }

        return default(T);
    }

    protected virtual void FixedUpdateAct() { }
    protected virtual void UpdateAct() { }

    protected virtual void Death()
    {
        _rigidbody.detectCollisions = false;

        StopAllCoroutines();

        HealthChangeEvent = null;
    }

    protected void InvokeChangeHealthEvent()
    {
        _healthChangeEventArgs.MaxHealth = _maxHealth;
        _healthChangeEventArgs.CurHealth = _curHealth;
        HealthChangeEvent?.Invoke(this, _healthChangeEventArgs);
    }

    private void TakeDamageHelper(float damage, float criticalMultiplier, float criticalRate)
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

    private bool CalculateCritical(float criticalRate)
    {
        if (0f >= criticalRate) return false;

        if (UnityEngine.Random.Range(0f, 100f) >= criticalRate) return false;

        return true;
    }
}

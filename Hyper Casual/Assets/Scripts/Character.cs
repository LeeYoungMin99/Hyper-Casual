using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _attackDamage = 100f;
    [SerializeField] protected float _maxHealth = 1000;
    [SerializeField] protected float _curHealth = 1000;

    public event EventHandler<HealthChangeEventArgs> HealthChangeEvent;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    private List<StatusEffect> _statusEffects = new List<StatusEffect>();
    private HealthChangeEventArgs _healthChangeEventArgs = new HealthChangeEventArgs();
    private bool _isDead;
    private bool _canAct;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _rigidbody.detectCollisions = true;
    }

    private void FixedUpdate()
    {
        if (true == _isDead) return;

        _canAct = true;

        int count = _statusEffects.Count;
        for (int i = 0; i < count; ++i)
        {
            if (false == _statusEffects[i].Update(this))
            {
                _canAct = false;
            }
        }

        _animator.SetBool(AnimationID.CAN_ACT, _canAct);
        if (false == _canAct) return;

        FixedUpdateAct();
    }

    private void Update()
    {
        if (false == _canAct) return;

        UpdateAct();
    }

    private void LateUpdate()
    {
        if (false == _canAct) return;

        LateUpdateAct();
    }

    public void TakeDamage(float damage,
                           float criticalMultiplier,
                           float criticalRate)
    {
        bool isCritical = false;

        if (true == CalculateCritical(criticalRate))
        {
            damage *= criticalMultiplier;
            isCritical = true;
            CameraShaker.Instance.ShakeCamera(0.5f, 0.5f);
        }

        TakeDamageHelper(damage, isCritical);
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

    protected virtual void LateUpdateAct() { }

    protected virtual void Death()
    {
        if (true == _isDead) return;

        Destroy(gameObject, 2f);

        _animator.SetBool(AnimationID.IS_DEAD, true);

        _isDead = true;
        _canAct = false;
        _rigidbody.detectCollisions = false;
        _rigidbody.velocity = Utils.ZERO_VECTOR3;

        StopAllCoroutines();

        HealthChangeEvent = null;
    }

    protected void InvokeChangeHealthEvent()
    {
        _healthChangeEventArgs.MaxHealth = _maxHealth;
        _healthChangeEventArgs.CurHealth = _curHealth;
        HealthChangeEvent?.Invoke(this, _healthChangeEventArgs);
    }

    protected virtual void TakeDamageHelper(float damage, bool isCritical)
    {
        DamageTextManager.Instance.MarkDamageText(this, damage, isCritical);
        _curHealth -= damage;

        InvokeChangeHealthEvent();

        if (0.01f < _curHealth) return;

        Death();
    }

    private bool CalculateCritical(float criticalRate)
    {
        if (0f >= criticalRate) return false;

        if (UnityEngine.Random.Range(0f, 100f) >= criticalRate) return false;

        return true;
    }
}

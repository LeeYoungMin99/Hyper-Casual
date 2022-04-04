using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public float MaxHealth;
    public float CurHealth;
    public float Damage;
    public float MoveSpeed;

    public event EventHandler<HealthChangeEventArgs> HealthChangeEvent;

    protected Rigidbody _rigidbody;
    protected Animator _animator;

    private HealthChangeEventArgs _healthChangeEventArgs = new HealthChangeEventArgs();
    private bool _isFreeze = false;
    private bool _isBlaze = false;
    private bool _isPoisonous = false;

    private const float DURATION = 2f;
    private const float BLAZE_DAMAGE_MULTIPLIER = 0.18f;
    private const float BLAZE_INTERVAL_TIME = 2f;
    private const float BLAZE_TICK_COUNT = 2f;
    private const float POISON_DAMAGE_MULTIPLIER = 0.35f;
    private const float POISON_INTERVAL_TIME = 2f;

    public abstract void Death();

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _rigidbody.detectCollisions = true;

        _isFreeze = false;
        _isBlaze = false;
        _isPoisonous = false;
    }

    public void TakeDamage(float damage,
                           float criticalMultiplier,
                           float criticalRate,
                           bool isFreeze,
                           bool isBlaze,
                           bool isPoisonous)
    {
        if (true == isFreeze)
        {
            if (false == _isFreeze)
            {
                StartCoroutine(Freeze());
            }
        }

        if (true == isBlaze)
        {
            if (false == _isBlaze)
            {
                StartCoroutine(Blaze(damage * BLAZE_DAMAGE_MULTIPLIER, criticalMultiplier, criticalRate));
            }
        }

        if (true == isPoisonous)
        {
            if (false == _isPoisonous)
            {
                StartCoroutine(Poison(damage * POISON_DAMAGE_MULTIPLIER, criticalMultiplier, criticalRate));
            }
        }

        TakeDamageHelper(damage, criticalMultiplier, criticalRate);
    }

    public IEnumerator Freeze()
    {
        _isFreeze = true;

        yield return new WaitForSeconds(DURATION);

        _isFreeze = false;
    }

    public IEnumerator Blaze(float damage, float criticalMultiplier, float criticalRate)
    {
        _isBlaze = true;

        for (int i = 0; i < BLAZE_TICK_COUNT; ++i)
        {
            yield return new WaitForSeconds(BLAZE_INTERVAL_TIME);

            TakeDamageHelper(damage, criticalMultiplier, criticalRate);
        }

        _isBlaze = false;
    }

    public IEnumerator Poison(float damage, float criticalMultiplier, float criticalRate)
    {
        _isPoisonous = true;

        while (true)
        {
            yield return new WaitForSeconds(POISON_INTERVAL_TIME);

            TakeDamageHelper(damage, criticalMultiplier, criticalRate);
        }
    }

    public bool CalculateCritical(float criticalRate)
    {
        if (0f >= criticalRate) return false;

        if (UnityEngine.Random.Range(0f, 100f) >= criticalRate) return false;

        return true;
    }

    public void TakeDamageHelper(float damage, float criticalMultiplier, float criticalRate)
    {
        bool isCritical = false;

        if (true == CalculateCritical(criticalRate))
        {
            damage *= criticalMultiplier;
            isCritical = true;
            StartCoroutine(CameraShaker.Instance.ShakeCamera(0.5f));
        }

        DamageTextManager.Instance.MarkDamageText(this, damage, isCritical);
        CurHealth -= damage;

        InvokeChangeHealthEvent();

        if (0f < CurHealth) return;

        _rigidbody.detectCollisions = false;

        StopAllCoroutines();

        HealthChangeEvent = null;

        Death();
    }

    public void InvokeChangeHealthEvent()
    {
        _healthChangeEventArgs.MaxHealth = MaxHealth;
        _healthChangeEventArgs.CurHealth = CurHealth;
        HealthChangeEvent?.Invoke(this, _healthChangeEventArgs);
    }
}

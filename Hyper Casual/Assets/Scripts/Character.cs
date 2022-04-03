using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    protected Rigidbody _rigidbody;

    public float MaxHealth;
    public float Health;
    public float Damage;

    public bool IsFreeze = false;
    public bool IsBlaze = false;
    public bool IsPoisonous = false;

    private const float DURATION = 2f;
    private const float BLAZE_DAMAGE_MULTIPLIER = 0.18f;
    private const float BLAZE_INTERVAL_TIME = 2f;
    private const float BLAZE_TICK_COUNT = 2f;
    private const float POISON_DAMAGE_MULTIPLIER = 0.35f;
    private const float POISON_INTERVAL_TIME = 2f;

    public abstract void Death();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        _rigidbody.detectCollisions = true;

        IsFreeze = false;
        IsBlaze = false;
        IsPoisonous = false;
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
            if (false == IsFreeze)
            {
                StartCoroutine(Freeze());
            }
        }

        if (true == isBlaze)
        {
            if (false == IsBlaze)
            {
                StartCoroutine(Blaze(damage * BLAZE_DAMAGE_MULTIPLIER, criticalMultiplier, criticalRate));
            }
        }

        if (true == isPoisonous)
        {
            if (false == IsPoisonous)
            {
                StartCoroutine(Poison(damage * POISON_DAMAGE_MULTIPLIER, criticalMultiplier, criticalRate));
            }
        }

        Debug.Log($"일반 피해 {damage}");

        TakeDamageHelper(damage, criticalMultiplier, criticalRate);
    }

    public IEnumerator Freeze()
    {
        IsFreeze = true;

        yield return new WaitForSeconds(DURATION);

        IsFreeze = false;
    }

    public IEnumerator Blaze(float damage, float criticalMultiplier, float criticalRate)
    {
        for (int i = 0; i < BLAZE_TICK_COUNT; ++i)
        {
            yield return new WaitForSeconds(BLAZE_INTERVAL_TIME);

            TakeDamageHelper(damage, criticalMultiplier, criticalRate);

            Debug.Log($"불 피해 {damage}");
        }
    }

    public IEnumerator Poison(float damage, float criticalMultiplier, float criticalRate)
    {
        while (true)
        {
            yield return new WaitForSeconds(POISON_INTERVAL_TIME);

            TakeDamageHelper(damage, criticalMultiplier, criticalRate);

            Debug.Log($"독 피해 {damage}");
        }
    }

    public bool CalculateCritical(float criticalRate)
    {
        if (0f >= criticalRate) return false;

        if (Random.Range(0f, 100f) >= criticalRate) return false;

        return true;
    }

    public void TakeDamageHelper(float damage, float criticalMultiplier, float criticalRate)
    {
        if (true == CalculateCritical(criticalRate))
        {
            damage *= criticalMultiplier;
        }

        Health -= damage;

        if (0f < Health) return;

        _rigidbody.detectCollisions = false;

        StopAllCoroutines();

        Death();
    }
}

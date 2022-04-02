using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public float Health;
    public float Damage;

    public abstract void Death();

    public void TakeDamage(float damage, float criticalMultiplier, float criticalRate)
    {
        if (Random.Range(0f, 100f) > criticalRate)
        {
            damage *= criticalMultiplier;
        }

        Health -= damage;

        Debug.Log($"{damage} ÇÇÇØ");

        if (0 < Health) return;

        Death();
    }
}

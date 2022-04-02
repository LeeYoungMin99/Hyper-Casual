using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IDamageable
{
    public float Health;
    public float Damage;

    public abstract void Death();

    public void TakeDamage(float damage)
    {
        Health -= damage;

        Debug.Log($"{damage} ÇÇÇØ");

        if (0 < Health) return;

        Death();
    }
}

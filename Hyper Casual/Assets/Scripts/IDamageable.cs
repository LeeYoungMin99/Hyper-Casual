using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage,
                           float criticalMultiplier = 2f,
                           float criticalRate = 0f);
}

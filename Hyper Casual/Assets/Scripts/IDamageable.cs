using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage,
                           float criticalMultiplier,
                           float criticalRate,
                           bool isFreeze,
                           bool isBlaze,
                           bool isPoisonous);
}

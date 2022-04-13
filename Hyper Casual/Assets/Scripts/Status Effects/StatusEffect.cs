using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public abstract void Activate(Character character, float duration, float damage, float criticalMultiplier, float criticalRate);
}

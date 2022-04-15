using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatusEffectTag { Blaze, Poison, Freeze }

public abstract class StatusEffect
{
    protected EStatusEffectTag _tag;

    public abstract void Activate(Character character, float duration, float damage, float criticalMultiplier, float criticalRate);
}

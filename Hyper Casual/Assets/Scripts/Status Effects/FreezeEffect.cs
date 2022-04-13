using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect
{
    private float _duration;
    private float _prevActivateTime;

    public override void Activate(Character character, float duration, float damage, float criticalMultiplier, float criticalRate)
    {
        if (Time.time < _prevActivateTime + _duration) return;

        _duration = duration;
        _prevActivateTime = Time.time;

        character.ActivateStatusEffect(EStatusEffectType.Freeze, _duration, 0, 0, 0);
    }
}

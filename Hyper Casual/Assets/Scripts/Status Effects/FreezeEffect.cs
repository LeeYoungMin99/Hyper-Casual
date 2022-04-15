using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect
{
    private float _duration;
    private float _prevActivateTime;

    public FreezeEffect()
    {
        _tag = EStatusEffectTag.Freeze;
    }

    public override void Activate(Character character, float duration, float damage, float criticalMultiplier, float criticalRate)
    {
        if (Time.time < _prevActivateTime + _duration) return;

        _duration = duration;
        _prevActivateTime = Time.time;

        character.ActivateStatusEffect(_tag, _duration, 0, 0, 0);
    }
}

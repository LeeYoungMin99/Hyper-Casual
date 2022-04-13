using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeEffect : StatusEffect
{
    private float _damage;

    public override void Activate(Character character, float duration, float damage, float criticalMultiplier, float criticalRate)
    {
        if (_damage > damage) return;

        _damage = damage;

        character.ActivateStatusEffect(EStatusEffectType.Blaze, duration, _damage, criticalMultiplier, criticalRate);
    }
}

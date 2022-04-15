using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeEffect : StatusEffect
{
    private float _damage;

    public BlazeEffect()
    {
        _tag = EStatusEffectTag.Blaze;
    }

    public override void Activate(Character character, 
                                  float duration, 
                                  float damage, 
                                  float criticalMultiplier, 
                                  float criticalRate)
    {
        if (_damage > damage) return;

        _damage = damage;

        character.ActivateStatusEffect(_tag, 
                                       duration, 
                                       _damage, 
                                       criticalMultiplier, 
                                       criticalRate);
    }
}

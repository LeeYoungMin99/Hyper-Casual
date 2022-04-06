using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeEffect : StatusEffect
{
    private float _duration;
    private float _elapsedTime;

    private float _damage;
    private float _criticalMultiplier;
    private float _criticalRate;

    private const float INTERVAL = 0.25f;

    public void Init(float duration, float damage, float criticalMultiplier, float criticalRate)
    {
        if (_damage > damage) return;

        _duration = duration;
        _damage = damage;
        _criticalMultiplier = criticalMultiplier;
        _criticalRate = criticalRate;
        _elapsedTime = 0f;
    }

    public override bool Update(Character character)
    {
        if (0f >= _duration) return true;

        _duration -= Time.deltaTime;
        _elapsedTime += Time.deltaTime;

        if (INTERVAL > _elapsedTime) return true;

        _elapsedTime -= INTERVAL;

        character.TakeDamage(_damage, _criticalMultiplier, _criticalRate);

        return true;
    }
}

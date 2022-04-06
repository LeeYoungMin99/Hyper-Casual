using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : StatusEffect
{
    private float _elapsedTime;

    private float _damage;
    private float _criticalMultiplier;
    private float _criticalRate;

    private const float INTERVAL = 1f;

    public void Init(float damage, float criticalMultiplier, float criticalRate)
    {
        if (_damage > damage) return;

        _damage = damage;
        _criticalMultiplier = criticalMultiplier;
        _criticalRate = criticalRate;
        _elapsedTime = 0f;
    }

    public override bool Update(Character character)
    {
        _elapsedTime += Time.deltaTime;

        if (INTERVAL > _elapsedTime) return true;

        _elapsedTime -= INTERVAL;

        character.TakeDamage(_damage, _criticalMultiplier, _criticalRate);

        return true;
    }
}

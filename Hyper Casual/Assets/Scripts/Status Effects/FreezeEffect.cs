using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect
{
    private bool _enable;
    private float _duration;

    public void Init(float duration)
    {
        if (0 < _duration) return;

        _duration = duration;
        _enable = true;
    }

    public override bool Update(Character character)
    {
        if (0 >= _duration || false == _enable) return true;

        _duration -= Time.deltaTime;

        return false;
    }

    public void Unfreeze()
    {
        _enable = false;
    }
}

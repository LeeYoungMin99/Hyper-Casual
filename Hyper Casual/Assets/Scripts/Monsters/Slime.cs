using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    private KingSlime _parent;

    protected override void Awake()
    {
        base.Awake();

        RunAndBounceWall runAndBounceWall = new RunAndBounceWall(_stateMachine, this, 500f);
        new SplitBody(_stateMachine, this);

        _stateMachine.ChangeState(runAndBounceWall);

        _parent = GetComponentInParent<KingSlime>();
    }

    protected override void TakeDamageHelper(float damage, bool isCritical)
    {
        DamageTextManager.Instance.MarkDamageText(this, damage, isCritical);
        _curHealth -= damage;

        InvokeChangeHealthEvent();

        if (0f >= _curHealth)
        {
            if (damage < _curHealth * -1)
            {
                damage = _curHealth = 0f;
            }

            _parent.TakeDamage(damage + _curHealth, 0f, 0f);

            Death();
        }
        else
        {
            _parent.TakeDamage(damage, 0f, 0f);
        }
    }
}

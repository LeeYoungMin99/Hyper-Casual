using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    protected override void Awake()
    {
        base.Awake();

        HealthBarManager.Instance.CreateHealthBar(this, EHealthBarType.Monster);
        InvokeChangeHealthEvent();
    }

    protected override void Death()
    {
        base.Death();

        ExperienceManager.Instance.CreateExperience(1000f, transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Weapon Weapon;

    public float AttackSpeed = 1f;
    public float CriticalMultiplier = 2f;
    public float CriticalRate = 0f;

    public int AttackCount = 1;
    public int FrontFireCount = 1;
    public int RearFireCount = 1;
    public int SideFireCount = 1;
    public int DiagonalFireCount = 1;

    public float t = 30f;

    private void Awake()
    {
        Weapon = new Knife();
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (AttackSpeed <= t)
        {
            Weapon.Attack(transform,
                          Damage,
                          CriticalMultiplier,
                          CriticalRate,
                          FrontFireCount,
                          RearFireCount,
                          SideFireCount,
                          DiagonalFireCount);

            t = 0f;
        }
    }

    public override void Death()
    {

    }

}
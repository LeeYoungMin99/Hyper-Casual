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
    public int ForwardFireCount = 1;
    public int BackwardFireCount = 1;
    public int SideFireCount = 1;
    public int DiagonalFireCount = 1;

    private void Awake()
    {
        Weapon = new Knife(transform);
    }

    private void Update()
    {
        Weapon.Attack(Damage,
                      CriticalMultiplier,
                      CriticalRate,
                      ForwardFireCount,
                      BackwardFireCount,
                      SideFireCount,
                      DiagonalFireCount);
    }

    public override void Death()
    {
       
    }

}
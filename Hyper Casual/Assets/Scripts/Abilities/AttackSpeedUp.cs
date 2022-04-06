using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUp : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.AttackSpeedUp(1.25f);
    }
}
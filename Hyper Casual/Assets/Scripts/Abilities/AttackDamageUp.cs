using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageUp : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.AttackDamageUp(1.2f);
    }
}

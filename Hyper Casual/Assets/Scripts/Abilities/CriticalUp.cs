using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalUp : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.CriticalUp(1.4f, 20f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthUp : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.MaxHealthUp(1.2f);
    }
}

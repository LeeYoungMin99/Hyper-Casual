using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearArrow : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.RearArrow();
    }
}

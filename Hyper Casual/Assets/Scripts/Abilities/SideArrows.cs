using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideArrows : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.SideArrows();
    }
}

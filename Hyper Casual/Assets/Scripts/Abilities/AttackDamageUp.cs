using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageUp : Ability
{
    public override void ApplyAbility(Player character, Weapon weapon)
    {
        character.AttackDamageUp(1.2f);
    }

    public override void InvokeAbility(Transform transform, Collider other, float criticalMultiplier, float CriticalRate, ref float damage, ref int wallBounce, ref int monsterBounce) { }
}

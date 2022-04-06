using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationID
{
    public static readonly int IS_MOVE = Animator.StringToHash("IsMove");
    public static readonly int IS_ATTACK = Animator.StringToHash("IsAttack");
    public static readonly int ATTACK_SPEED = Animator.StringToHash("AttackSpeed");
    public static readonly int IS_DEAD = Animator.StringToHash("IsDead");
    public static readonly int CAN_ACT = Animator.StringToHash("CanAct");
}

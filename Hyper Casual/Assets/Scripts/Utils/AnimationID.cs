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
    public static readonly int DASH = Animator.StringToHash("Dash");
    public static readonly int SLASH = Animator.StringToHash("Slash");
    public static readonly int SHOT = Animator.StringToHash("Shot");
    public static readonly int AIMING = Animator.StringToHash("Aiming");
    public static readonly int CHASE = Animator.StringToHash("Chase");
}

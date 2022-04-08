using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _moveSpeed = 300f;
    [SerializeField] private float _criticalMultiplier = 2f;
    [SerializeField] private float _criticalRate = 0f;
    [SerializeField] private int _attackCount = 1;
    [SerializeField] private int _frontAttackCount = 1;
    [SerializeField] private int _rearAttackCount = 1;
    [SerializeField] private int _sideAttackCount = 1;
    [SerializeField] private int _frontDiagonalAttackCount = 1;
    [SerializeField] private int _rearDiagonalAttackCount = 0;

    private Weapon _weapon;
    private Collider _target;
    private Collider _collider;
    private RaycastHit _hit;
    private bool _isMove = false;
    private float _attackSpeed = 1f;

    private const float ATTACK_INTERVAL_TIME = 0.1f;
    private const float SEARCH_DISTANCE = 50f;
    private const string ENABLE_COLLIDER = "EnableCollider";

    protected override void Awake()
    {
        base.Awake();

        HealthBarManager.Instance.CreateHealthBar(this, EHealthBarType.Player);
        InvokeChangeHealthEvent();

        _weapon = new Knife(this);
        _collider = GetComponent<Collider>();

        SlotMachine slotMachine = GameObject.Find("Slot Machine Canvas").transform.
                                             Find("Slot Machine").GetComponent<SlotMachine>();

        slotMachine.AbilityGainEvent -= ApplyAbility;
        slotMachine.AbilityGainEvent += ApplyAbility;
    }

    protected override void FixedUpdateAct()
    {
        float x = _joystick.Horizontal;
        float z = _joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0f, z);

        _isMove = false;

        if (0 != moveVec.sqrMagnitude)
        {
            _isMove = true;

            StopAllCoroutines();

            moveVec *= _moveSpeed * Time.deltaTime;

            Quaternion dirQuat = Quaternion.LookRotation(moveVec);
            Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, 0.3f);

            _rigidbody.MoveRotation(moveQuat);
            _rigidbody.velocity = moveVec;
        }

        _animator.SetBool(AnimationID.IS_MOVE, _isMove);
    }

    protected override void UpdateAct()
    {
        if (true == _isMove || null == _target)
        {
            _target = FindNearTarget();

            _animator.SetBool(AnimationID.IS_ATTACK, false);

            return;
        }

        if (false == _target.attachedRigidbody.detectCollisions)
        {
            _target = FindNearTarget();

            return;
        }

        _animator.SetBool(AnimationID.IS_ATTACK, true);
    }

    private void LateUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerValue.WALL_LAYER == collision.gameObject.layer) return;

        _collider.enabled = false;
        Invoke(ENABLE_COLLIDER, 1f);
    }

    public void AttackDamageUp(float multiplier)
    {
        _attackDamage *= multiplier;
    }

    public void AttackSpeedUp(float multiplier)
    {
        _attackSpeed *= multiplier;

        _animator.SetFloat(AnimationID.ATTACK_SPEED, _attackSpeed);
    }

    public void CriticalUp(float multiplier, float rate)
    {
        _criticalMultiplier *= multiplier;
        _criticalRate += rate;
    }

    public void MaxHealthUp(float multiplier)
    {
        _maxHealth *= multiplier;
        _curHealth *= multiplier;

        InvokeChangeHealthEvent();
    }

    public void MultiShot()
    {
        _attackCount += 1;
    }

    public void FrontArrow()
    {
        _frontAttackCount += 1;
    }

    public void DiagonalArrows()
    {
        _frontDiagonalAttackCount += 1;
    }

    public void SideArrows()
    {
        _sideAttackCount += 1;
    }

    public void RearArrow()
    {
        _rearAttackCount += 1;
    }

    protected override void Death()
    {
        base.Death();
    }

    private void Attack()
    {
        AttackHelper(_attackCount);
    }

    private void AttackHelper(int count)
    {
        _weapon.Attack(transform,
                      _attackDamage,
                      _criticalMultiplier,
                      _criticalRate,
                      _frontAttackCount,
                      _rearAttackCount,
                      _sideAttackCount,
                      _frontDiagonalAttackCount,
                      _rearDiagonalAttackCount);

        if (2 > count) return;

        StartCoroutine(ExtraAttack(count - 1));
    }

    private IEnumerator ExtraAttack(int attackCount)
    {
        yield return new WaitForSeconds(ATTACK_INTERVAL_TIME);

        AttackHelper(attackCount);

        --attackCount;

        if (0 >= attackCount) yield break;

        StartCoroutine(ExtraAttack(attackCount));
    }

    private void LookatTarget()
    {
        Vector3 targetDir = _target.transform.position - transform.position;

        Quaternion moveQuat = Quaternion.LookRotation(targetDir);

        _rigidbody.MoveRotation(moveQuat);
    }

    private Collider FindNearTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(Vector3.zero, SEARCH_DISTANCE, Utils.Colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        float minDistance = float.MaxValue;

        if (0 == count) return null;

        Collider target = Utils.Colliders[0];

        if (1 == count) return target;

        for (int i = 0; i < count; ++i)
        {
            float distance = Vector3.Distance(Utils.Colliders[i].transform.position, transform.position);

            if (minDistance <= distance) continue;

            Vector3 targetDir = Utils.Colliders[i].transform.position - transform.position;

            Physics.Raycast(transform.position, targetDir, out _hit, SEARCH_DISTANCE);

            if (LayerValue.WALL_LAYER == _hit.transform.gameObject.layer) continue;

            minDistance = distance;
            target = Utils.Colliders[i];
        }

        return target;
    }

    private void ApplyAbility(object sender, AbilityEventArgs args)
    {
        args.Ability.ApplyAbility(this, _weapon);
    }

    private void EnableCollider()
    {
        _collider.enabled = true;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Joystick _joystick;

    public Weapon Weapon;

    public float AttackSpeed = 1f;
    public float CriticalMultiplier = 2f;
    public float CriticalRate = 0f;

    public int AttackCount = 1;
    public int FrontFireCount = 1;
    public int RearFireCount = 1;
    public int SideFireCount = 1;
    public int DiagonalFireCount = 1;

    private Collider[] _colliders = new Collider[16];
    private Coroutine _extraAttackCoroutine;
    private Collider _target;
    private RaycastHit _hit;
    private Ray _ray = new Ray();
    private bool _isMove = false;

    private const float ATTACK_INTERVAL_TIME = 0.1f;
    private const float SEARCH_DISTANCE = 50f;
    protected override void Awake()
    {
        base.Awake();

        Weapon = new Knife();
    }

    private void FixedUpdate()
    {
        float x = _joystick.Horizontal;
        float z = _joystick.Vertical;

        Vector3 moveVec = new Vector3(x, 0f, z);

        if (0 == moveVec.sqrMagnitude)
        {
            _isMove = false;
        }
        else
        {
            _isMove = true;

            if (null != _extraAttackCoroutine)
            {
                StopCoroutine(_extraAttackCoroutine);
            }

            moveVec *= MoveSpeed * Time.deltaTime;

            Quaternion dirQuat = Quaternion.LookRotation(moveVec);
            Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, 0.3f);

            _rigidbody.MoveRotation(moveQuat);

            if (false == Physics.Raycast(_rigidbody.position, moveVec, 1f, LayerValue.MAP_OBJECT_LAYER_MASK))
            {
                _rigidbody.MovePosition(_rigidbody.position + moveVec);
            }
        }

        _animator.SetBool(AnimationID.IS_MOVE, _isMove);
    }

    private void Update()
    {
        if (true == _isMove || null == _target)
        {
            _target = FindNearTarget();

            _animator.SetBool(AnimationID.IS_ATTACK, false);

            return;
        }

        Vector3 targetDir = _target.transform.position - transform.position;

        _ray.origin = transform.position;
        _ray.direction = targetDir;

        if (false == _target.Raycast(_ray, out _hit, SEARCH_DISTANCE))
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

    public override void Death()
    {

    }

    private void Attack()
    {
        AttackHelper(AttackCount);
    }

    private void AttackHelper(int count)
    {
        Weapon.Attack(transform,
                          Damage,
                          CriticalMultiplier,
                          CriticalRate,
                          FrontFireCount,
                          RearFireCount,
                          SideFireCount,
                          DiagonalFireCount);

        if (2 > count) return;

        _extraAttackCoroutine = StartCoroutine(ExtraAttack(count - 1));
    }

    private IEnumerator ExtraAttack(int attackCount)
    {
        yield return new WaitForSeconds(ATTACK_INTERVAL_TIME);

        AttackHelper(attackCount);

        --attackCount;

        if (0 >= attackCount) yield break;

        _extraAttackCoroutine = StartCoroutine(ExtraAttack(attackCount));
    }

    private void LookatTarget()
    {
        Vector3 targetDir = _target.transform.position - transform.position;

        Quaternion moveQuat = Quaternion.LookRotation(targetDir);

        _rigidbody.MoveRotation(moveQuat);
    }

    private Collider FindNearTarget()
    {
        int count = Physics.OverlapSphereNonAlloc(Vector3.zero, SEARCH_DISTANCE, _colliders, LayerValue.ALL_ENEMY_LAYER_MASK);

        float minDistance = float.MaxValue;

        if (0 == count) return null;

        Collider target = _colliders[0];

        if (1 == count) return target;

        for (int i = 0; i < count; ++i)
        {
            float distance = Vector3.Distance(_colliders[i].transform.position, transform.position);

            if (minDistance <= distance) continue;

            Vector3 targetDir = _colliders[i].transform.position - transform.position;

            Physics.Raycast(transform.position, targetDir, out _hit, SEARCH_DISTANCE);

            if (LayerValue.MAP_OBJECT_LAYER == _hit.transform.gameObject.layer) continue;

            minDistance = distance;
            target = _colliders[i];
        }

        return target;
    }
}
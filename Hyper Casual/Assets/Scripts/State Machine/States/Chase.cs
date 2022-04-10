using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform _target;
    private Rigidbody _rigidbody;
    private float _moveSpeed;
    private float _rotateSpeed;
    private float _elapsedTime;

    private const float INTERVAL_TIME = 0.25f;

    public Chase(StateMachine stateMachine, Character owner, float moveSpeed, float rotateSpeed) : base(stateMachine, owner)
    {
        _animator = owner.GetComponent<Animator>();
        _navMeshAgent = owner.GetComponent<NavMeshAgent>();
        _rigidbody = owner.GetComponent<Rigidbody>();
        _moveSpeed = moveSpeed;
        _rotateSpeed = rotateSpeed;

        Physics.OverlapSphereNonAlloc(_owner.transform.position, 100f, Utils.Colliders, LayerValue.ALL_PLAYER_LAYER_MASK);
        _target = Utils.Colliders[0].transform;
    }

    public override void Enter()
    {
        base.Enter();

        SetDestination();

        _animator.SetBool(AnimationID.CHASE, true);
    }

    public override void Exit()
    {
        _navMeshAgent.ResetPath();

        _rigidbody.velocity = Utils.ZERO_VECTOR3;

        _animator.SetBool(AnimationID.CHASE, false);
    }

    public override void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= INTERVAL_TIME)
        {
            SetDestination();
        }

        Vector3 targetDir = _navMeshAgent.steeringTarget - _rigidbody.position;
        Quaternion dirQuat = Quaternion.LookRotation(targetDir);
        Quaternion moveQuat = Quaternion.Slerp(_rigidbody.rotation, dirQuat, _rotateSpeed * Time.deltaTime);

        _rigidbody.MoveRotation(moveQuat);
        _rigidbody.velocity = targetDir.normalized * (_moveSpeed * Time.deltaTime);

        _transitionParameter.Distance = Vector3.Distance(_rigidbody.position, _target.position);
        _transitionParameter.Time += Time.deltaTime;
    }

    private void SetDestination()
    {
        Debug.Log("셋데스티네이션");
        _navMeshAgent.SetDestination(_target.position);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.updateRotation = false;

        _elapsedTime = 0f;
    }
}

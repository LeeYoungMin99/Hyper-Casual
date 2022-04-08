using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : State
{
    private LineRenderer _lineRenderer;
    private Transform _target;
    private Animator _animator;
    private RaycastHit _hit;

    public Aiming(StateMachine stateMachine, Character owner, int bounceCount = 1) : base(stateMachine, owner)
    {
        _animator = owner.GetComponent<Animator>();

        _lineRenderer = owner.GetComponent<LineRenderer>();
        _lineRenderer.positionCount = bounceCount + 1;

        Physics.OverlapSphereNonAlloc(owner.transform.position, 100f, Utils.Colliders, LayerValue.ALL_PLAYER_LAYER_MASK);
        _target = Utils.Colliders[0].transform;
    }

    public override void Enter()
    {
        base.Enter();

        _lineRenderer.enabled = true;

        _animator.SetBool(AnimationID.AIMING, true);
    }

    public override void Exit()
    {
        _lineRenderer.enabled = false;

        _animator.SetBool(AnimationID.AIMING, false);
    }

    public override void FixedUpdate()
    {
        Quaternion dirQuat = Quaternion.LookRotation(_target.position - _owner.transform.position);

        _owner.transform.rotation = dirQuat;

        Vector3 newPosition = _owner.transform.position;
        Vector3 newDir = _owner.transform.forward;

        _lineRenderer.SetPosition(0, newPosition);

        int count = _lineRenderer.positionCount;
        for (int i = 1; i < count; i++)
        {
            Physics.Raycast(newPosition, newDir, out _hit, 100f, LayerValue.WALL_AND_MAP_LAYER_MASK);

            newPosition = _hit.point;
            newDir = Vector3.Reflect(newDir, _hit.normal);

            _lineRenderer.SetPosition(i, newPosition);
        }

        _transitionParameter.Time += Time.deltaTime;
    }
}
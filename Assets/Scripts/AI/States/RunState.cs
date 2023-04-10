using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class RunState : State
{
    [SerializeField] private Animator _animator;
    [SerializeField] private StateMachineData _data;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _dashSpeed = 8;
    [SerializeField] private RunAnimationEvent _runAnimationEvent;

    private AudioSource _audioSource;
    private bool _eventWorked;
    private Vector3 _lastPosition;
    private DistanceTransition _distanceTransition;

    public override void Enter()
    {
        _runAnimationEvent.DashCompleted += OnDashCompleted;
        _runAnimationEvent.DashStarted += OnDashStarted;
        Scream();
        _audioSource = _data.PlayerController.GetComponent<AudioSource>();
        _lastPosition = _data.PlayerController.transform.position;
        
        transform.DOLookAt(_lastPosition, .5f);
    }

    private void Update()
    {
        if (IsRunning && _audioSource && _audioSource.isPlaying && _eventWorked)
            _agent.SetDestination(_data.PlayerController.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        _runAnimationEvent.DashCompleted -= OnDashCompleted;
        _runAnimationEvent.DashStarted -= OnDashStarted;
        _agent.SetDestination(_agent.transform.position);
        _eventWorked = false;
    }
    
    private void OnDashCompleted()
    {
        _agent.speed = _runSpeed;
        _animator.SetFloat(AnimationConstants.Speed, _runSpeed);
        _eventWorked = true;
        base.Enter();
    }

    private void OnDashStarted()
    {
        _agent.SetDestination(_lastPosition);
        _agent.speed = _dashSpeed;
    }

    private void Scream()
    {
        _animator.SetTrigger(AnimationConstants.Scream);
    }
}

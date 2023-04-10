using UnityEngine;
using UnityEngine.AI;

public class WalkState : State
{
    [SerializeField] private StateMachineData _data;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _walkSpeed;

    private Transform _currentWaypoint;

    public override void Enter()
    {
        base.Enter();
        SelectTarget(null);
    }
    
    public override void Exit()
    {
        base.Exit();
        _agent.SetDestination(_agent.transform.position);
    }

    private void SelectTarget(Transform current)
    {
        _currentWaypoint = _data.GetNextWaypoint(current);
        _agent.speed = _walkSpeed;
        _agent.SetDestination(_currentWaypoint.position);
        _animator.SetFloat(AnimationConstants.Speed, _walkSpeed);
    }
}

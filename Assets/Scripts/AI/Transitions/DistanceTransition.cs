using UnityEngine;
using UnityEngine.AI;

public class DistanceTransition : Transition
{
    [SerializeField] private NavMeshAgent _agent;

    private void Update()
    {
        if(IsRunning && _agent.remainingDistance <= _agent.stoppingDistance)
            Trigger();
    }
}

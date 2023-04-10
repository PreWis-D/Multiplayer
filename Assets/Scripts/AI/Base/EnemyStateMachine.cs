using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;

    private State _currentState;
    
    private void Start()
    {
        EnterState(_firstState);
    }

    private void EnterState(State state)
    {
        print(state);
        _currentState = state;
        _currentState.Enter();
        
        foreach (var transition in _currentState.Transitions)
        {
            transition.Triggered += OnTransitionTriggered;
            transition.Enable();
        }
    }

    private void ExitState(State state)
    {
        foreach (var transition in state.Transitions)
        {
            transition.Triggered -= OnTransitionTriggered;
            transition.Disable();
        }
        
        state.Exit();
        _currentState = null;
    }

    private void OnTransitionTriggered(State targetState)
    {
        ExitState(_currentState);
        EnterState(targetState);
    }
}

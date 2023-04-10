using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    public bool IsRunning { get; private set; }
    public IEnumerable<Transition> Transitions => _transitions;
    
    public virtual void Enter()
    {
        ActivateTransitions(true);
        IsRunning = true;
    }

    public virtual void Exit()
    {
        ActivateTransitions(false);
        IsRunning = false;
    }
    
    private void ActivateTransitions(bool targetValue)
    {
        foreach (var transition in _transitions)
        {
            transition.enabled = targetValue;
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;
    protected bool IsRunning { get; private set; }
    
    public State TargetState => _targetState;
    public bool NeedTransit { get; protected set; }
    public event UnityAction<State> Triggered;

    public virtual void Enable()
    {
        IsRunning = true; 
    }

    public virtual void Disable()
    {
        IsRunning = false;
    }

    public virtual void Reset()
    {
        NeedTransit = false;
    }

    protected void Trigger()
    {
        NeedTransit = true;
        Triggered?.Invoke(TargetState);
    }
}

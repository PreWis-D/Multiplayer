using UnityEngine;

public class StunToIdleTransition : Transition
{
    [SerializeField] private StunAnimationEvent _stunAnimationEvent;
    
    public override void Enable()
    {
        base.Enable();
        _stunAnimationEvent.StunFinished += OnStunFinished;
    }

    public override void Disable()
    {
        base.Disable();
        _stunAnimationEvent.StunFinished -= OnStunFinished;
    }

    private void OnStunFinished()
    {
        Trigger();
    }
}

using UnityEngine;

public class StunState : State
{
    [SerializeField] private Animator _animator;
    
    public override void Enter()
    {
        base.Enter();
        _animator.SetTrigger(AnimationConstants.Stun);
    }
}

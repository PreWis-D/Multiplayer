using UnityEngine;

public class IdleState : State
{
    [SerializeField] private Animator _animator;

    public override void Enter()
    {
        base.Enter();
        _animator.SetFloat(AnimationConstants.Speed, 0);
    }
}

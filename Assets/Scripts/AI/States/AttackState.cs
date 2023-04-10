using UnityEngine;

public class AttackState : State
{
    [SerializeField] private Animator _animator;
    [SerializeField] private StateMachineData _data;
    [SerializeField] private KillAnimationEvent _animationEvent;
    
    public override void Enter()
    {
        base.Enter();
        _animator.SetTrigger(AnimationConstants.Attack);

        _animationEvent.Invoked += Kill;
    }

    public override void Exit()
    {
        base.Exit();
        _animationEvent.Invoked -= Kill;
    }

    private void Kill()
    {
        if (_data.PlayerController.TryGetComponent(out PlayerDeath playerDeath))
        {
            playerDeath.Die(this);
            Debug.Log("Killed");
        }
        else 
            Debug.Log("There are no PlayerDeath component on target");
    }
}

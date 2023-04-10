using UnityEngine;

public class AttackTriggerTransition : Transition
{
    [SerializeField] private PlayerTrigger _playerTrigger;
    [SerializeField] private StateMachineData _data;

    public override void Enable()
    {
        base.Enable();
        _playerTrigger.Enter += OnEnter;
    }

    public override void Disable()
    {
        base.Disable();
        _playerTrigger.Enter -= OnEnter;
    }

    private void OnEnter(PlayerController controller)
    {
        _data.SetPlayerController(controller);
        Trigger();
    }
}

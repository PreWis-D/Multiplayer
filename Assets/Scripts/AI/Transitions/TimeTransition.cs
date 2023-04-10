using UnityEngine;

public class TimeTransition : Transition
{
    [SerializeField] private float _workTime;

    private float _timeLeft;

    private void Update()
    {
        if(!IsRunning)
            return;

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0)
            Trigger();
    }

    public override void Enable()
    {
        base.Enable();
        _timeLeft = _workTime;
    }

    public override void Disable()
    {
        base.Disable();
        _timeLeft = 0;
    }
}

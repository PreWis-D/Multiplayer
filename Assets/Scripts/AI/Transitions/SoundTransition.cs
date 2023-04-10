using UnityEngine;

public class SoundTransition : Transition
{
    [SerializeField] private StateMachineData _data;
    [SerializeField] private PlayerTrigger _playerTrigger;
    
    private PlayerController _controller;
    private AudioSource _audioSource;

    private void Start()
    {
        _playerTrigger.Disable();
    }

    public override void Enable()
    {
        base.Enable();
        _playerTrigger.Enter += OnPlayerTriggerEnter;
        _playerTrigger.Exit += OnPlayerTriggerExit;
        _playerTrigger.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        _playerTrigger.Enter -= OnPlayerTriggerEnter;
        _playerTrigger.Exit -= OnPlayerTriggerExit;
        _playerTrigger.Disable();
    }

    private void Update()
    {
        if (IsRunning && _controller != null && _audioSource.isPlaying)
        {
            _data.SetPlayerController(_controller);
            Trigger();
        }
    }

    private void OnPlayerTriggerEnter(PlayerController controller)
    {
        _controller = controller;
        _audioSource = _controller.GetComponent<AudioSource>();
    }

    private void OnPlayerTriggerExit(PlayerController controller)
    {
        _controller = null;
        _audioSource = null;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Ridgepole : MonoBehaviour
{
    [SerializeField] private PlayerTrigger _trigger;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _track;

    public event UnityAction Triggered;

    private void OnEnable()
    {
        _trigger.Enter += OnEnter;
        _trigger.Enable();
    }

    private void OnDisable()
    {
        _trigger.Enter -= OnEnter;
        _trigger.Disable();
    }

    private void OnEnter(PlayerController controller)
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_track);
            Triggered?.Invoke();
        }
    }
}

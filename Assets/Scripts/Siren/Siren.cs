using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour
{
    [SerializeField] private List<Ridgepole> _ridgepoles;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _track;

    private void OnEnable()
    {
        foreach (var ridgepole in _ridgepoles)
        {
            ridgepole.Triggered += OnEnter;
        }
    }

    private void OnDisable()
    {
        foreach (var ridgepole in _ridgepoles)
        {
            ridgepole.Triggered -= OnEnter;
        }
    }

    private void OnEnter()
    {
        if(!_audioSource.isPlaying)
            _audioSource.PlayOneShot(_track);
    }
}

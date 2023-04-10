using DG.Tweening;
using UnityEngine;

public class RotateInteractable : InteractableObject
{
    [SerializeField] private float _openedAngle = 90f;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _rotateDuration = .7f;
    [SerializeField] private bool _openOnAwake;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private RotateAxis _rotateAxis;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isOpened;
    private Vector3 _targetAngleOpened;
    
    private void Awake()
    {
        switch (_rotateAxis)
        {
            case RotateAxis.X: _targetAngleOpened = Vector3.right;
                break;
            case RotateAxis.Y: _targetAngleOpened = Vector3.up;
                break;
            case RotateAxis.Z: _targetAngleOpened = Vector3.forward;
                break;
        }
        
        _targetAngleOpened *= _openedAngle;
        
        if(_openOnAwake)
            Rotate(0, false);
    }

    public override void Execute()
    {
        base.Execute();
        Rotate(_rotateDuration, true);
    }

    private void Rotate(float duration, bool playSound)
    {
        Vector3 targetAngle = _isOpened ? _targetAngleOpened : -_targetAngleOpened;;

        targetAngle += _targetTransform.localRotation.eulerAngles;
        _targetTransform.DOLocalRotate(targetAngle, duration);
        _isOpened = !_isOpened;

        if (playSound)
        {
            _audioSource.PlayOneShot(_audioClip);

            if (_particleSystem)
                _particleSystem.Play();
        }
    }
}

public enum RotateAxis
{
    X , Y, Z
}



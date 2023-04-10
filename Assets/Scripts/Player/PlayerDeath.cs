using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _disableOnDeath;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _lerpSpeed;

    private List<Camera> _cameras = new List<Camera>();
    private int _spectateID = -1;
    private Camera _currentCamera;

    public bool IsDead { get; private set; }
    public event UnityAction<PlayerDeath> Dead;

    public void Die(AttackState attackState)
    {
        IsDead = true;
        Dead?.Invoke(this);

        foreach (var component in _disableOnDeath)
        {
            component.enabled = false;
        }
        
        SelectCamera();
        StartCoroutine(Spectate());
    }

    public void SetCameras(IEnumerable<Camera> cameras)
    {
        _cameras = cameras.Where(c => c != Camera.current).ToList();
    }

    private void SelectCamera()
    {
        _spectateID++;

        if (_cameras.Count <= 1)
            throw new Exception("There are only one player in lobby");

        if (_spectateID >= _cameras.Count)
            _spectateID = 0;

        _currentCamera = _cameras[_spectateID];
    }

    private IEnumerator Spectate()
    {
        Transform cameraTransform = Camera.current.transform;
        
        _playerInput.ChangeSpectatableKeyClicked += SelectCamera;
        
        while (IsDead)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, _currentCamera.transform.position, _lerpSpeed);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, _currentCamera.transform.rotation, _lerpSpeed);
            yield return null;
        }
        
        _playerInput.ChangeSpectatableKeyClicked -= SelectCamera;
    }
}

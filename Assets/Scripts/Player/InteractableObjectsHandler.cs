using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using System.Security.Cryptography;

public class InteractableObjectsHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _handObj;
    [SerializeField] private Transform _hand;

    [SerializeField] private LayerMask _animColliderMask;
    [SerializeField] private float _takeDistance;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Inventory _inventory;

    private RaycastHit _hit;
    private InteractableObject _interactableObject;
    private CollectableObject _collectableObject;

    public event UnityAction<string> ObjectRaycastReached;
    public event UnityAction ObjectRaycastAbandoned;
    public event UnityAction<CollectableObject> ItemPickedUp;

    private new void OnEnable()
    {
        base.OnEnable();
        _playerInput.TakeKeyClicked += OnTakeClicked;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _playerInput.TakeKeyClicked -= OnTakeClicked;
    }


    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out _hit, _takeDistance, _animColliderMask))
        {
            if (_hit.transform)
                TryGetComponent(_hit);
        }
        else
        {
            TryUnLinkObjects();
        }
    }

    private void OnTakeClicked()
    {
        if (_interactableObject)
        {
            if (_collectableObject != null)
            {
                _handObj = _hit.collider.gameObject;

                if (_handObj.TryGetComponent(out Flashlight flashlight))
                {
                    ItemPickedUp?.Invoke(flashlight);
                }
                else if (_handObj.TryGetComponent(out DetectorCollectable detectorCollectable))
                {
                    ItemPickedUp?.Invoke(detectorCollectable);
                }
                else if (_handObj.TryGetComponent(out KeyCard keyCard))
                {
                    ItemPickedUp?.Invoke(keyCard);
                }
                else if (_handObj.TryGetComponent(out Grenade grenade))
                {
                    ItemPickedUp?.Invoke(grenade);
                }
            }
            else
            {
                _interactableObject.Execute();
            }
        }
    }

    private void TryGetComponent(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out InteractableObject targetObject))
        {
            if (_interactableObject == null)
            {
                _interactableObject = targetObject.transform.GetComponent<InteractableObject>();
                _collectableObject = _interactableObject.transform.GetComponent<CollectableObject>();
                ObjectRaycastReached?.Invoke(_interactableObject.Description);
            }
        }
    }

    private void TryUnLinkObjects()
    {
        if (_interactableObject)
        {
            _interactableObject = null;
            ObjectRaycastAbandoned?.Invoke();
        }

        if (_collectableObject)
            _collectableObject = null;
    }
}

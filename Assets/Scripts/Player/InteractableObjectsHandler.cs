using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class InteractableObjectsHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _handObj;
    [SerializeField] private Transform _hand;

    [SerializeField] private LayerMask _animColliderMask;
    [SerializeField] private float _takeDistance;
    [SerializeField] private PlayerInput _playerInput;

    private RaycastHit _hit;
    private InteractableObject _interactableObject;

    public event UnityAction<string> ObjectRaycastReached;
    public event UnityAction ObjectRaycastAbandoned;

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
            _interactableObject.Execute();
        }
    }

    private void TryGetComponent(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out InteractableObject targetObject))
        {
            if (_interactableObject == null)
            {
                _interactableObject = targetObject.transform.GetComponent<InteractableObject>();
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
    }
}

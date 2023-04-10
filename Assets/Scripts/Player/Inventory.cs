using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class Inventory : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _hand;
    [SerializeField] private InteractableObjectsHandler _objectsHandler;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _throwPower = 1f;
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private PlayerDeath _playerDeath;

    [Header("Items")]
    [SerializeField] private Flashlight _flashlight;
    [SerializeField] private DetectorCollectable _detector;
    [SerializeField] private KeyCard _keyCard;
    [SerializeField] private Grenade _grenade;

    private List<CollectableObject> _playerCopyItems = new List<CollectableObject>();
    private List<CollectableObject> _items = new List<CollectableObject>();
    private CollectableObject _currentItem;
    private bool _isInventoryOpened = false;
    private Grenade _currentGrenade;

    public List<CollectableObject> Items => _items;

    public event UnityAction<CollectableObject> ItemAdded;
    public event UnityAction<CollectableObject> ItemRemoved;
    public event UnityAction<CollectableObject> ItemCountChanged;

    #region ItemsTaken
    public event UnityAction<bool> FlashlightTaken;
    public event UnityAction<bool> DetectorTaken;
    public event UnityAction<bool> KeyCardTaken;
    public event UnityAction<bool> GrenadeTaken;
    #endregion

    #region GrenadeEvents
    public event UnityAction<bool> GrenadePreparation;
    public event UnityAction<bool> CancelGrenadeThrow;
    public event UnityAction GrenadeThrow;
    #endregion

    private new void OnEnable()
    {
        base.OnEnable();
        _playerInput.ThroableKeyClicked += OnThroableKeyClicked;
        _playerInput.GetFlashlightKeyClicked += OnGetFlashlightKeyClicked;
        _playerInput.Mouse0KeyClicked += OnMouse0KeyClicked;
        _playerInput.Mouse0KeyUp += OnMouse0KeyUp;
        _playerInput.Mouse1KeyClicked += OnMouse1KeyClicked;
        _playerInput.GetDetectorKeyClicked += OnGetDetectorKeyClicked;
        _objectsHandler.ItemPickedUp += OnItemPickedUp;
        _playerDeath.Dead += OnPlayerDead;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _playerInput.ThroableKeyClicked -= OnThroableKeyClicked;
        _playerInput.GetFlashlightKeyClicked -= OnGetFlashlightKeyClicked;
        _playerInput.Mouse0KeyClicked -= OnMouse0KeyClicked;
        _playerInput.Mouse0KeyUp -= OnMouse0KeyUp;
        _playerInput.Mouse1KeyClicked -= OnMouse1KeyClicked;
        _playerInput.GetDetectorKeyClicked -= OnGetDetectorKeyClicked;
        _objectsHandler.ItemPickedUp -= OnItemPickedUp;
        _playerDeath.Dead -= OnPlayerDead;
    }

    private void Start()
    {
        _playerCopyItems.Add(_flashlight);
        _playerCopyItems.Add(_detector);
        _playerCopyItems.Add(_keyCard);
        _playerCopyItems.Add(_grenade);
    }

    public void DropItem(CollectableObject collectableObject)
    {
        foreach (var item in _items)
        {
            if (item.Key == collectableObject.Key)
            {
                if (_flashlight.Key == item.Key)
                {
                    _flashlight.ChangeLockState(true);
                    if (_flashlight.gameObject.activeSelf)
                        photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

                    if (_currentItem != null && _currentItem.Key == _flashlight.Key)
                        _currentItem = null;
                }

                if (_detector.Key == item.Key)
                {
                    _detector.Scan(false);
                    _detector.ChangeLockState(true);
                    if (_detector.gameObject.activeSelf)
                        photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

                    if (_currentItem != null && _currentItem.Key == _detector.Key)
                        _currentItem = null;
                }

                if (_keyCard.Key == item.Key)
                {
                    _keyCard.ChangeLockState(true);
                    if (_keyCard.gameObject.activeSelf)
                        photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

                    if (_currentItem != null && _currentItem.Key == _keyCard.Key)
                        _currentItem = null;
                }

                if (_grenade.Key == item.Key)
                {
                    if (_grenade.Count < 1)
                    {
                        _grenade.ChangeLockState(true);

                        if (_grenade.gameObject.activeSelf)
                            photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

                        if (_currentItem != null && _currentItem.Key == _grenade.Key)
                            _currentItem = null;
                    }
                }

                photonView.RPC(nameof(ThrowObj), RpcTarget.All, item.PhotonViewID);
                _items.Remove(item);
                ItemRemoved?.Invoke(item);
                break;
            }
        }
    }

    public void TryItemUsed(CollectableObject collectableObject)
    {
        if (collectableObject.Key == _flashlight.Key)
            OnGetFlashlightKeyClicked();
        else if (collectableObject.Key == _detector.Key)
            OnGetDetectorKeyClicked();
        else if (collectableObject.Key == _keyCard.Key)
            OnGetKeyCardClicked();
        else if (collectableObject.Key == _grenade.Key)
            OnGetGrenadeClicked();
    }

    public void ChangeInventoryViewOpenedState(bool value)
    {
        _isInventoryOpened = value;
    }

    public void TryDropItem(CollectableObject collectable)
    {
        foreach (var playerItem in _playerCopyItems)
        {
            if (collectable.Key == playerItem.Key)
            {
                if (playerItem.Count > 1)
                {
                    playerItem.RemoveCount();
                    DropItem(playerItem);
                    ItemCountChanged?.Invoke(playerItem);
                }
                else
                {
                    playerItem.ResetCount();
                    DropItem(playerItem);
                    ItemRemoved?.Invoke(playerItem);
                }
            }
        }
    }

    private void OnPlayerDead(PlayerDeath death)
    {
        foreach (var item in _items)
        {
            TryDropItem(item);
        }
    }

    private void OnGetFlashlightKeyClicked()
    {
        if (_flashlight.Islock == false)
        {
            if (_currentItem != null && _currentItem.Key != _flashlight.Key)
                photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

            photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _flashlight.PhotonViewID);
        }
    }

    private void OnGetDetectorKeyClicked()
    {
        if (_detector.Islock == false)
        {
            if (_currentItem != null && _currentItem.Key != _detector.Key)
                photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

            photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _detector.PhotonViewID);
        }
    }

    private void OnGetKeyCardClicked()
    {
        if (_keyCard.Islock == false)
        {
            if (_currentItem != null && _currentItem.Key != _keyCard.Key)
                photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

            photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _keyCard.PhotonViewID);
        }
    }

    private void OnGetGrenadeClicked()
    {
        if (_grenade.Islock == false && _grenade.Count > 0)
        {
            if (_currentItem != null && _currentItem.Key != _grenade.Key)
                photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);

            foreach (var item in _items)
            {
                if (item.Key == _grenade.Key)
                {
                    if (item.TryGetComponent(out Grenade grenade))
                        _currentGrenade = grenade;
                }
            }

            photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _grenade.PhotonViewID);
        }
    }

    private void OnMouse0KeyClicked()
    {
        if (_flashlight.gameObject.activeSelf && !_isInventoryOpened)
            _flashlight.ChangeLight();

        if (_grenade.gameObject.activeSelf && !_isInventoryOpened)
        {
            _currentGrenade.ChangeThrowPower();
            GrenadePreparation?.Invoke(true);
        }
    }

    private void OnMouse1KeyClicked()
    {
        if (_grenade.gameObject.activeSelf && !_isInventoryOpened)
        {
            _currentGrenade.OnThrowCanceled();
            CancelGrenadeThrow?.Invoke(false);
        }
    }

    private void OnMouse0KeyUp()
    {
        if (!_isInventoryOpened && _currentGrenade != null)
        {
            if (_grenade.gameObject.activeSelf && _currentGrenade.IsPreparation)
            {
                GrenadeThrow?.Invoke();
                photonView.RPC(nameof(ThrowObj), RpcTarget.All, _currentGrenade.PhotonViewID);
                RemoveItem(_currentGrenade);
                UnLinkCurrentItem(_currentGrenade);
            }
        }
    }

    private void OnThroableKeyClicked()
    {
        if (_items.Count > 0 && _currentItem != null)
        {
            foreach (var item in _items)
            {
                if (_currentItem.Key == item.Key)
                {
                    if (_currentItem.Key == _detector.Key)
                        _detector.Scan(false);

                    RemoveItem(item);
                    photonView.RPC(nameof(ThrowObj), RpcTarget.All, item.PhotonViewID);
                    UnLinkCurrentItem(item);
                    break;
                }
            }
        }
    }

    private void OnItemPickedUp(CollectableObject collectable)
    {
        _items.Add(collectable);

        if (collectable.TryGetComponent(out Flashlight flashlight))
            _flashlight.ChangeLockState(false);

        if (collectable.TryGetComponent(out DetectorCollectable detector))
            _detector.ChangeLockState(false);

        if (collectable.TryGetComponent(out KeyCard keyCard))
            _keyCard.ChangeLockState(false);

        if (collectable.TryGetComponent(out Grenade grenade))
        {
            _grenade.ChangeLockState(false);
            _grenade.AddCount();
            ItemAdded?.Invoke(_grenade);
        }
        else
        {
            ItemAdded?.Invoke(collectable);
        }

        photonView.RPC(nameof(SetParent), RpcTarget.All, collectable.PhotonViewID, _hand.gameObject.GetPhotonView().ViewID);
    }

    private void RemoveItem(CollectableObject item)
    {
        if (item.Count < 1)
        {
            ItemRemoved?.Invoke(_currentItem);
            _currentItem.ChangeLockState(true);
        }

        _items.Remove(item);
        photonView.RPC(nameof(TryChangeItemState), RpcTarget.All, _currentItem.PhotonViewID);
    }

    private void UnLinkCurrentItem(CollectableObject item)
    {
        _currentItem = null;

        if (item == _currentGrenade)
            _currentGrenade = null;
    }

    [PunRPC]
    private void TryChangeItemState(int objID)
    {
        CollectableObject collectable = PhotonNetwork.GetPhotonView(objID).gameObject.GetComponent<CollectableObject>();

        if (collectable.gameObject.activeSelf)
        {
            collectable.gameObject.SetActive(false);
            if (_currentItem == collectable)
                _currentItem = null;

            if (collectable.gameObject.TryGetComponent(out KeyCard keyCard))
                KeyCardTaken?.Invoke(false);

            if (collectable.gameObject.TryGetComponent(out Grenade grenade))
                GrenadeTaken?.Invoke(false);

            if (collectable.gameObject.TryGetComponent(out Flashlight flashlight))
                FlashlightTaken?.Invoke(false);

            if (collectable.gameObject.TryGetComponent(out DetectorCollectable detector))
            {
                _detector.Scan(false);
                DetectorTaken?.Invoke(false);
            }
        }
        else
        {
            collectable.gameObject.SetActive(true);
            _currentItem = collectable;

            if (collectable.gameObject.TryGetComponent(out KeyCard keyCard))
                KeyCardTaken?.Invoke(true);

            if (collectable.gameObject.TryGetComponent(out Grenade grenade))
                GrenadeTaken?.Invoke(true);

            if (collectable.gameObject.TryGetComponent(out Flashlight flashlight))
                FlashlightTaken?.Invoke(true);

            if (collectable.gameObject.TryGetComponent(out DetectorCollectable detector))
            {
                _detector.Scan(true);
                DetectorTaken?.Invoke(true);
            }
        }
    }

    [PunRPC]
    private void SetParent(int childViewID, int parentViewID)
    {
        CollectableObject child = PhotonNetwork.GetPhotonView(childViewID).gameObject.GetComponent<CollectableObject>();
        Transform parent = PhotonNetwork.GetPhotonView(parentViewID).gameObject.transform;
        child.OnTaked(parent);
        child.gameObject.SetActive(false);
    }

    [PunRPC]
    private void ThrowObj(int objID)
    {
        CollectableObject collectable = PhotonNetwork.GetPhotonView(objID).gameObject.GetComponent<CollectableObject>();
        collectable.gameObject.SetActive(true);

        if (collectable == _currentGrenade)
        {
            _grenade.RemoveCount();
            ItemCountChanged?.Invoke(_grenade);
            _currentGrenade.StartTimer();
        }

        collectable.OnThrow();
    }
}
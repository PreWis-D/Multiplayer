using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InteractableObjectsHandler _interactableHandler;

    [Header("Panels")]
    [SerializeField] private SprintBar _sprintBar;
    [SerializeField] private InventoryView _inventory;
    [SerializeField] private MessagePanel _messagePanel;

    private bool _isInventoryOpened = false;

    private new void OnEnable()
    {
        base.OnEnable();
        _interactableHandler.ObjectRaycastReached += OnObjectReached;
        _interactableHandler.ObjectRaycastAbandoned += OnObjectAbandoned;
        _playerInput.InventoryKeyClicked += OnInventoryClicked;
        _inventory.Hided += OnInventoryHided;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _interactableHandler.ObjectRaycastReached -= OnObjectReached;
        _interactableHandler.ObjectRaycastAbandoned -= OnObjectAbandoned;
        _playerInput.InventoryKeyClicked -= OnInventoryClicked;
        _inventory.Hided -= OnInventoryHided;
    }

    private void OnInventoryClicked()
    {
        if (_isInventoryOpened == false)
        {
            _playerController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            _inventory.Show();
            _isInventoryOpened = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inventory.Hide();
            _playerController.enabled = true;
            _isInventoryOpened = false;
        }
    }

    private void OnInventoryHided()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerController.enabled = true;
        _isInventoryOpened = false;
    }

    private void OnObjectReached(string description)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _messagePanel.Show();
        _messagePanel.ShowInfo(description);
    }

    private void OnObjectAbandoned()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        _messagePanel.Hide();
        _messagePanel.ShowInfo("");
    }
}

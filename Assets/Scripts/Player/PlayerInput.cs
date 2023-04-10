using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera _playerCamera;

    public KeyCode SprintKey { get; private set; } = KeyCode.LeftShift;
    public KeyCode CrouchKey { get; private set; } = KeyCode.LeftControl;
    public KeyCode JumpKey { get; private set; } = KeyCode.Space;

    public KeyCode TakeKey { get; private set; } = KeyCode.E;
    public KeyCode ThroableKey { get; private set; } = KeyCode.Q;
    public KeyCode InventoryKey { get; private set; } = KeyCode.I;
    public KeyCode GetFlashlightKey { get; private set; } = KeyCode.F;
    public KeyCode GetDetectorKey { get; private set; } = KeyCode.R;
    public KeyCode Mouse0Key { get; private set; } = KeyCode.Mouse0;
    public KeyCode Mouse1Key { get; private set; } = KeyCode.Mouse1;
    public KeyCode ChangeSpectatable { get; private set; } = KeyCode.Mouse0;

    public Vector3 MoveVector3 { get; set; }
    public float CameraX { get; set; }
    public float CameraY { get; set; }

    public event UnityAction SprintKeyClicked;
    public event UnityAction SprintKeyUp;
    public event UnityAction CrouchKeyClicked;
    public event UnityAction JumpKeyClicked;
    public event UnityAction TakeKeyClicked;
    public event UnityAction ThroableKeyClicked;
    public event UnityAction InventoryKeyClicked;
    public event UnityAction GetFlashlightKeyClicked;
    public event UnityAction GetDetectorKeyClicked;
    public event UnityAction Mouse0KeyClicked;
    public event UnityAction Mouse0KeyUp;
    public event UnityAction Mouse1KeyClicked;
    public event UnityAction ChangeSpectatableKeyClicked;

    private void Start()
    {
        _playerCamera.enabled = photonView.IsMine;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        MoveVector3 = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        CameraX = Input.GetAxis("Mouse X");
        CameraY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(SprintKey))
            SprintKeyClicked?.Invoke();

        if (Input.GetKeyUp(SprintKey))
            SprintKeyUp?.Invoke();

        if (Input.GetKeyDown(CrouchKey))
            CrouchKeyClicked?.Invoke();

        if (Input.GetKeyDown(JumpKey))
            JumpKeyClicked?.Invoke();

        if (Input.GetKeyDown(TakeKey))
            TakeKeyClicked?.Invoke();

        if (Input.GetKeyDown(ThroableKey))
            ThroableKeyClicked?.Invoke();

        if (Input.GetKeyDown(InventoryKey))
            InventoryKeyClicked?.Invoke();

        if (Input.GetKeyDown(GetFlashlightKey))
            GetFlashlightKeyClicked?.Invoke();

        if (Input.GetKeyDown(GetDetectorKey))
            GetDetectorKeyClicked?.Invoke();

        if (Input.GetKeyDown(Mouse0Key))
            Mouse0KeyClicked?.Invoke();

        if (Input.GetKeyUp(Mouse0Key))
                Mouse0KeyUp?.Invoke();

        if (Input.GetKeyDown(Mouse1Key))
            Mouse1KeyClicked?.Invoke();

        if (Input.GetKeyDown(ChangeSpectatable))
            ChangeSpectatableKeyClicked?.Invoke();
    }
}

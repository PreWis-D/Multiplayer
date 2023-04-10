using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class PlayerAnimator : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerController _personController;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private MultiAimConstraint _aimConstraint;

    private PhotonView _photonView;
    private Animator _animator;
    private int _velocittHashX;
    private int _velocityHashZ;
    private float _speedZ;
    private float _speedX;

    private const string SittingAnim = "IsSitting";
    private const string MoveJumpAnim = "MoveJump";
    private const string IdleJumpAnim = "IdleJump";
    private const string FlashlightAnim = "IsFlashlight";
    private const string DetectorAnim = "IsDetector";
    private const string KeyCardAnim = "IsKeyCard";
    private const string GrenadeIdleAnim = "IsGrenadeIdle";
    private const string GrenadeThrowAnim = "GrenadeThrow";
    private const string PreparationForTrowGrenadeAnim = "PreparationForTrowGrenade";

    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        _playerInput.CrouchKeyClicked += OnCrouchKeyClicked;
        _personController.Jumped += OnJumpKeyClicked;
        _inventory.FlashlightTaken += OnFlashlightTaken;
        _inventory.DetectorTaken += OnDetectorTaken;
        _inventory.KeyCardTaken += OnKeyCardTaken;
        _inventory.GrenadeTaken += OnGrenadeTaken;
        _inventory.GrenadePreparation += OnGrenadePreparation;
        _inventory.CancelGrenadeThrow += OnGrenadePreparation;
        _inventory.GrenadeThrow += OnGrenadeThrow;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _playerInput.CrouchKeyClicked -= OnCrouchKeyClicked;
        _personController.Jumped -= OnJumpKeyClicked;
        _inventory.FlashlightTaken -= OnFlashlightTaken;
        _inventory.DetectorTaken -= OnDetectorTaken;
        _inventory.KeyCardTaken -= OnKeyCardTaken;
        _inventory.GrenadeTaken -= OnGrenadeTaken;
        _inventory.GrenadePreparation -= OnGrenadePreparation;
        _inventory.CancelGrenadeThrow -= OnGrenadePreparation;
        _inventory.GrenadeThrow -= OnGrenadeThrow;
    }

    private void Start()
    {
        _velocittHashX = Animator.StringToHash("Velocity X");
        _velocityHashZ = Animator.StringToHash("Velocity Z");
        _aimConstraint.weight = 0;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        photonView.RPC(nameof(SetAnimFloat), RpcTarget.All, _photonView.ViewID, velocity.x * _personController.currentSpeed, velocity.z * _personController.currentSpeed);
    }

    private void OnCrouchKeyClicked()
    {
        if (!Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!_animator.GetBool(SittingAnim))
                photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, SittingAnim, true);
            else if (_animator.GetBool(SittingAnim) || Input.GetKeyDown(_playerInput.JumpKey))
                photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, SittingAnim, false);
        }
    }

    private void OnJumpKeyClicked()
    {
        if (_personController.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
                photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, _photonView.ViewID, MoveJumpAnim);
            else
                photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, _photonView.ViewID, IdleJumpAnim);
        }
    }

    private void OnFlashlightTaken(bool value)
    {
        photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, FlashlightAnim, value);
    }

    private void OnDetectorTaken(bool value)
    {
        photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, DetectorAnim, value);
    }

    private void OnKeyCardTaken(bool value)
    {
        photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, KeyCardAnim, value);
    }

    private void OnGrenadeTaken(bool value)
    {
        photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, GrenadeIdleAnim, value);
    }

    private void OnGrenadePreparation(bool value)
    {
        photonView.RPC(nameof(SetAnimBool), RpcTarget.All, _photonView.ViewID, PreparationForTrowGrenadeAnim, value);
    }

    private void OnGrenadeThrow()
    {
        photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, _photonView.ViewID, GrenadeThrowAnim);
        OnGrenadePreparation(false);
    }

    [PunRPC]
    private void SetAnimBool(int GoViewID, string animName, bool value)
    {
        _aimConstraint.weight = value ? 1 : 0;
        Animator animator = PhotonNetwork.GetPhotonView(GoViewID).GetComponent<PlayerAnimator>().Animator;
        animator.SetBool(animName, value);
    }

    [PunRPC]
    private void SetAnimFloat(int GoViewID, float x, float z)
    {
        Animator animator = PhotonNetwork.GetPhotonView(GoViewID).GetComponent<PlayerAnimator>().Animator;
        animator.SetFloat(_velocittHashX, x);
        animator.SetFloat(_velocityHashZ, z);
    }

    [PunRPC]
    private void SetAnimTrigger(int GoViewID, string animName)
    {
        Animator animator = PhotonNetwork.GetPhotonView(GoViewID).GetComponent<PlayerAnimator>().Animator;
        animator.SetTrigger(animName);
    }
}

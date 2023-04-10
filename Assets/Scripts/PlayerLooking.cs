using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerLooking : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensivity = 1f;
    [SerializeField] private float _smoothing = 2f;

    //[SerializeField] private GameObject _handObj;
    //[SerializeField] private Transform _hand;
    //[SerializeField] private float _takeDistance;
    //[SerializeField] private LayerMask _animColliderMask;
    //[SerializeField] private float _throwPower;

    private float _speedRotation = 8f;
    private Vector2 _currentMouseLook;
    private Vector2 _appliedMouseDelta;
    private float _lookAngle;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)(transform.localEulerAngles.x * 100f));
        }
        else
        {
            _lookAngle = (int)stream.ReceiveNext() / 100f;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera.enabled = photonView.IsMine;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            RefreshMultiplayerStates();
            return;
        }

        Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * _sensivity * _smoothing);
        _appliedMouseDelta = Vector2.Lerp(_appliedMouseDelta, smoothMouseDelta, 1 / _smoothing);
        _currentMouseLook += _appliedMouseDelta;
        _currentMouseLook.y = Mathf.Clamp(_currentMouseLook.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-_currentMouseLook.y, Vector3.right);
        _player.localRotation = Quaternion.AngleAxis(_currentMouseLook.x, Vector3.up);

        //Debug.DrawRay(transform.position, transform.forward, Color.green);

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Ray ray = new Ray(transform.position, transform.forward);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, _takeDistance * 3, _animColliderMask))
        //    {
        //        _handObj = hit.collider.gameObject;
        //        Debug.Log(_handObj);
        //        photonView.RPC("SetParent", RpcTarget.All, _handObj.GetPhotonView().ViewID, _hand.gameObject.GetPhotonView().ViewID);   
        //    }
        //    else
        //    {
        //        Debug.Log("hit null");
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    photonView.RPC("ThrowObj", RpcTarget.All, _handObj.GetPhotonView().ViewID);
        //}
    }

    //[PunRPC]
    //private void SetParent(int childViewID, int parentViewID)
    //{
    //    Transform child = PhotonNetwork.GetPhotonView(childViewID).gameObject.transform;
    //    Transform parent = PhotonNetwork.GetPhotonView(parentViewID).gameObject.transform;
    //    child.position = parent.position;
    //    child.parent = parent;
    //    PhotonNetwork.GetPhotonView(childViewID).gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //}

    //[PunRPC]
    //private void ThrowObj(int objID)
    //{
    //    Rigidbody rigidbody = PhotonNetwork.GetPhotonView(objID).gameObject.GetComponent<Rigidbody>();
    //    PhotonNetwork.GetPhotonView(objID).gameObject.transform.parent = null;
    //    rigidbody.isKinematic = false;
    //    rigidbody.velocity = transform.forward * _throwPower;
    //    rigidbody.constraints = RigidbodyConstraints.None;
    //    _handObj = null;
    //}

    private void RefreshMultiplayerStates()
    {
        float cachEulY = transform.localEulerAngles.y;

        Quaternion targetRotation = Quaternion.identity * Quaternion.AngleAxis(_lookAngle, Vector3.right);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speedRotation * Time.deltaTime);

        Vector3 finalRotation = transform.localEulerAngles;
        finalRotation.y = cachEulY;

        transform.localEulerAngles = finalRotation;
    }
}

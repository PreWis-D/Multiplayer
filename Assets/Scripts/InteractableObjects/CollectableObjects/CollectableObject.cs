using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : InteractableObject
{
    [SerializeField] protected Rigidbody Rigidbody;

    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CellInfo _cell;
    [SerializeField] private string _key;
    [SerializeField] private int _count;

    [SerializeField] protected int ThrowPower = 0;
    private bool _islock = true;

    public int PhotonViewID => _photonView.ViewID;
    public int Count => _count;
    public bool Islock => _islock;
    public string Key => _key;
    public CellInfo Cell => _cell;

    public void OnTaked(Transform parent)
    {
        transform.position = parent.transform.position;
        transform.parent = parent.transform;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 90f));
        Rigidbody.isKinematic = true;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }

    public void OnThrow()
    {
        transform.parent = null;
        Rigidbody.isKinematic = false;
        Rigidbody.velocity = transform.forward * ThrowPower;
        Rigidbody.constraints = RigidbodyConstraints.None;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void ChangeLockState(bool value)
    {
        _islock = value;
    }

    public void ResetCount()
    {
        _count = 0;
    }

    public void AddCount()
    {
        _count++;
    }

    public void RemoveCount()
    {
        if (_count < 1)
            return;
        
        _count--;
    }
}
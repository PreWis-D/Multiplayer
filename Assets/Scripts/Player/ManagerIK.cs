using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerIK : MonoBehaviour
{
    [SerializeField] private bool _isIKActive = false;
    [SerializeField] private Transform _target;
    [SerializeField] private float _lookWeight;

    private Animator _animator;
    private GameObject _pivot;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _pivot = new GameObject("Pivot");
        _pivot.transform.parent = transform.parent;
        _pivot.transform.localPosition = new Vector3(transform.position.x, 0.54f, transform.position.z);
    }

    private void OnAnimatorIK()
    {
        if (_animator)
        {
            if (_isIKActive)
            {
                if (_target != null)
                {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(_target.position);
                }
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }
        }
    }
}

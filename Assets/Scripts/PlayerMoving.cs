using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _shiftSpeed;

    [Header("GroundCheckSetting")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    private float _antiGravity = -1.1f;
    private bool _isGrounded;
    private float _gravity = -9.8f;
    private Vector3 _velocity;
    private float _lastSpeed;

    private float _jumpHeight = 3f;

    private void Awake()
    {
        _lastSpeed = _speed;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGrounded == true && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        _characterController.Move(move * _speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * _antiGravity * _gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift))
            _speed = _shiftSpeed;
        else
            _speed = _lastSpeed;
    }
}

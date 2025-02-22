using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementExperimentation : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 5f;
    private float speed;
    [SerializeField] private float groundDrag = 7f;
    private Vector2 moveInput;
    
    [Header("Rotation Parameters")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float upDownRotationRange = 90f;
    [Space(5)]
    [SerializeField] Transform orienter;
    [SerializeField] private Transform rotator;
    [SerializeField] private Camera playerCamera;
    
    [Header("Jump Parameters")]
    [SerializeField] private float initialJumpForce = 5f;
    [SerializeField] private float jumpDampeningFactor = 0.6f;
    [SerializeField] private int jumpCount = 5;
    private int _jumpsRemaining;
    private List<float> _jumps = new();
    [Space(5)]
    [SerializeField] private float gravity = 350f;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    private bool _grounded;
    
    private Rigidbody _playerRigidbody;
    
    private PlayerInputHandler _playerInputHandler;

    private Vector3 _moveDirection;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        
        for (int i = 0; i < jumpCount; i++)
        {
            _jumps.Add(initialJumpForce * (float)Math.Pow(jumpDampeningFactor, i));
        }
        _jumpsRemaining = jumpCount;
    }

    private void Update()
    {
        SpeedDetermination();
        GroundCheck();
        HandleDrag();

        MoveInput();
        PlayerLook();
        
        Debug.Log(@$"Speed: {speed}, Move Input: {_playerInputHandler.MoveInput}, 
                    Look Input: {_playerInputHandler.LookInput}, Sprint Input: {_playerInputHandler.SprintValue}");
    }

    private void FixedUpdate()
    {
        MovePlayer();
        // Jump();
    }

    void SpeedDetermination()
    {
        speed = _playerInputHandler.SprintValue == 0f ? walkSpeed : walkSpeed * sprintMultiplier;
    }

    void GroundCheck()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);
    }

    void HandleDrag()
    {
        if (_grounded)
        {
            _playerRigidbody.linearDamping = groundDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = 0f;
        }
    }

    void MoveInput()
    {
        moveInput = _playerInputHandler.MoveInput * (speed * Time.deltaTime);
    }

    void MovePlayer()
    {
        _moveDirection = orienter.forward * moveInput.y + orienter.right * moveInput.x;
        
        _playerRigidbody.AddForce(_moveDirection.normalized * (speed), ForceMode.Force);
    }
    void PlayerLook()
    {
        // separate channels to enable sensitivity splitting into x and y values
        Vector2 mouseInput = new Vector2(_playerInputHandler.LookInput.x ,
            _playerInputHandler.LookInput.y) * (mouseSensitivity * Time.deltaTime);
        
        
    }

    void JumpInput()
    {
        
    }
    void Jump()
    {
        _playerRigidbody.AddForce(Vector3.up * (_jumps[jumpCount - _jumpsRemaining]), ForceMode.Impulse);
    }

    void UpdateJumps()
    {
        for (int i = 0; i < jumpCount; i++)
        {
            _jumps[i] = initialJumpForce * (float)Math.Pow(jumpDampeningFactor, i);
        }
    }
}

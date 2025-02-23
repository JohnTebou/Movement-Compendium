using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MovementExperimentation : MonoBehaviour
{
    public enum MovementState
    {
        walking,
        running,
        air
    }

    public MovementState state;
    
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 5f;
    private float speed;
    
    [Space(5)]
    [SerializeField] private float groundDrag = 7f;
    [SerializeField] private float airDrag = 0f;
    [SerializeField] private float airControlMultiplier = .6f;
    private Vector2 moveInput;
    
    [Header("Rotation Parameters")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float upDownRotationRange = 90f;
    private float pitch;
    private float yaw;
    
    [Space(5)]
    [SerializeField] Transform orienter;
    [SerializeField] private Transform rotator;
    [SerializeField] private Camera playerCamera;
    
    [Header("Jump Parameters")]
    public float initialJumpForce = 5f;
    public float jumpDampeningFactor = 0.6f;
    public int jumpCount = 5;
    public int _jumpsRemaining;
    public List<float> _jumps = new();
    private bool jump;
    
    [Space(5)]
    [SerializeField] private float gravity = 350f;
    
    [Header("Ground Check")]
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float playerHeightError;
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
        jump = false;
        
        Physics.gravity = new Vector3(0f, -gravity, 0f);
        
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    private void Update()
    {
        Debug.Log($"{_grounded}, Jumps Remaining: {_jumpsRemaining}, Max Jumps: {jumpCount}, Current Jump Force: {_jumps[Math.Min(jumpCount-_jumpsRemaining, _jumps.Count-1)]}");
        StateHandler();
        GroundCheck();
        HandleDrag();

        MoveInput();
        SpeedControl();
        PlayerLook();
        JumpInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    void GroundCheck()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, playerCollider.height/2 + playerHeightError, groundLayer);
    }

    void HandleDrag()
    {
        if (_grounded)
        {
            _playerRigidbody.linearDamping = groundDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = airDrag;
        }
    }

    private void StateHandler()
    {
        if (_grounded && _playerInputHandler.SprintValue == 0)
        {
            state = MovementState.walking;
            speed = walkSpeed;
        }
        else if (_grounded && _playerInputHandler.SprintValue != 0)
        {
            state = MovementState.running;
            speed = walkSpeed * sprintMultiplier;
        }
        else
        {
            state = MovementState.air;
        }
    }

    void MoveInput()
    {
        moveInput = _playerInputHandler.MoveInput * (speed * Time.deltaTime);
    }

    void MovePlayer()
    {
        _moveDirection = orienter.forward * moveInput.y + orienter.right * moveInput.x;
        
        _playerRigidbody.AddForce(_moveDirection.normalized * (speed * 10f * (_grounded ? 1 : airControlMultiplier)), ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatvel = new Vector3(_playerRigidbody.linearVelocity.x, 0, _playerRigidbody.linearVelocity.z);

        if (flatvel.magnitude > speed)
        {
            Vector3 limitedVelocity = flatvel.normalized * speed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }
    
    void PlayerLook()
    {
        // separate channels to enable sensitivity splitting into x and y values
        Vector2 mouseInput = new Vector2(_playerInputHandler.LookInput.x ,
            _playerInputHandler.LookInput.y) * (mouseSensitivity * Time.deltaTime);

        pitch -= mouseInput.y;
        pitch = Mathf.Clamp(pitch, -upDownRotationRange, upDownRotationRange);
        
        yaw += mouseInput.x;
        
        orienter.localRotation = Quaternion.Euler(orienter.rotation.x, yaw, orienter.rotation.z);
        rotator.localRotation = Quaternion.Euler(pitch, rotator.rotation.y, rotator.rotation.z);
    }

    void JumpInput()
    {
        if (_playerInputHandler.JumpTriggered && _jumpsRemaining > 0)
        {
            jump = true;
            _playerInputHandler.JumpTriggered = false;
        }

        if (_grounded)
        {
            _jumpsRemaining = jumpCount;
        }
        
        _jumpsRemaining = Mathf.Clamp(_jumpsRemaining, 0, jumpCount);
    }
    
    void Jump()
    {
        if (jump)
        {
            jump = false;
            _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0, _playerRigidbody.linearVelocity.z);
            _playerRigidbody.AddForce(Vector3.up * (_jumps[Math.Min(jumpCount - _jumpsRemaining, _jumps.Count-1)]), ForceMode.Impulse);
            _jumpsRemaining--;
        }
    }

    public void UpdateJumps()
    {
        _jumpsRemaining = jumpCount;
        _jumps = new List<float>(jumpCount);
        for (int i = 0; i < jumpCount; i++)
        {
            _jumps.Add(initialJumpForce * (float)Math.Pow(jumpDampeningFactor, i));
        }
    }
}

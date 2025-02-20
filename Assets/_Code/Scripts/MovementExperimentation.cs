using System;
using UnityEngine;

public class MovementExperimentation : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Input Controls")]
    
    
    [Header("Movement Values")]
    [SerializeField] private float jumpForce = 16.5f;

    private float jumpInputMultiplier = 10.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}

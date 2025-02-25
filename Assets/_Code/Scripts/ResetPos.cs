using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 resetPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        resetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.MovePosition(resetPos);
            rb.linearVelocity = Vector3.zero;
        }
    }
}

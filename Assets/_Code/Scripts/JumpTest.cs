using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JumpTest : MonoBehaviour
{
    [Header("Jump and Acceleration Text Controller")]
    [SerializeField] private TMP_InputField jumpText;
    [SerializeField] private TMP_InputField gravityText;

    [Header("Default Values")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravitationalAccelerationMagnitude;

    private Rigidbody rb;
    private bool jump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = false;

        jumpText.text = jumpForce.ToString();
        gravityText.text = gravitationalAccelerationMagnitude.ToString();
        Physics.gravity = new Vector3(0, -gravitationalAccelerationMagnitude, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (float.TryParse(jumpText.text.Trim(), out jumpForce))
        {
            Debug.Log("Jump Force: " + jumpText.text);
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number. You entered: " + jumpText.text);
        }

        if (float.TryParse(gravityText.text.Trim(), out gravitationalAccelerationMagnitude))
        {
            Debug.Log("Gravitational Acceleration: " + gravityText.text);
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number. You entered: " + gravityText.text);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        Physics.gravity = new Vector3(0, -gravitationalAccelerationMagnitude, 0);
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        jump = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}

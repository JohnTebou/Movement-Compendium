using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JumpTest_scrapped : MonoBehaviour
{
    [Header("Jump and Acceleration Text Controller")]
    [SerializeField] private TMP_InputField jumpText;
    [SerializeField] private TMP_InputField gravityText;
    private bool _gravityChanged;

    [Header("Default Values")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravitationalAccelerationMagnitude;

    private Rigidbody rb;
    private bool jump;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _gravityChanged = false;
        jump = false;

        jumpText.text = jumpForce.ToString();
        gravityText.text = gravitationalAccelerationMagnitude.ToString();
        Physics.gravity = new Vector3(0, -gravitationalAccelerationMagnitude, 0);

    }

    private void Update()
    {
        ValueParse(jumpForce, jumpText, "Jump Force: ");
        ValueParse(gravitationalAccelerationMagnitude, gravityText, "Gravitational Acceleration: ", flagbool: _gravityChanged);

        JumpInput();


        if (_gravityChanged)
        {
            Debug.Log("gan");
            UpdateGravity();
            _gravityChanged = false;
        }
        Debug.Log("gravityChanged: " + _gravityChanged);
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Jump();
        }
    }

    private void ValueParse(float oldval, TMP_InputField displayer, string regular_logtext, string error_logtext = "", bool flagbool = false) {
        string displayText = displayer.text;

        if (float.TryParse(displayText.Trim(), out float newVal))
        {
            if(oldval != newVal)
            {
                flagbool = true;
                Debug.Log(regular_logtext + ": " + displayText);
            }
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number. You entered: " + displayText);
        }
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    private void Jump()
    {
        jump = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
    private void UpdateGravity()
    {
        Physics.gravity = new Vector3(0, -gravitationalAccelerationMagnitude, 0);
        Debug.Log("Gravity Updated To: " + gravityText.text);
    }
}

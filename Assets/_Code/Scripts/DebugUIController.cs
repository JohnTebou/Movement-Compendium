using System;
using TMPro;
using UnityEngine;

namespace _Code.Scripts
{
    public class DebugUIController: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody playerRigidbody;
        
        [Header("Static Text Fields")]
        [SerializeField] private TextMeshProUGUI speedText;
        
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private TMP_InputField _jumpInputField;
        
        [Header("Controlled Values")]
        private float jumpMagnitude;

        private MovementExperimentation movementScript;
        private void Awake()
        {
            movementScript = GetComponent<MovementExperimentation>();
            
            _gravityInputField.onValueChanged.AddListener(OnGravityInputChanged);
            _jumpInputField.onValueChanged.AddListener(OnJumpInputChanged);
        }
        
        private void OnDestroy()
        {
            _gravityInputField.onValueChanged.RemoveListener(OnGravityInputChanged);
            _jumpInputField.onValueChanged.RemoveListener(OnJumpInputChanged);
        }

        private void Start()
        {
            _gravityInputField.text = Physics.gravity.magnitude.ToString();
            _jumpInputField.text = movementScript.initialJumpForce.ToString();
        }

        private void Update()
        {
            speedText.text = $"Flat Speed: {Math.Round(new Vector2(playerRigidbody.linearVelocity.z,playerRigidbody.linearVelocity.x).magnitude,3)}";
        }

        private void OnGravityInputChanged(string gravityInput)
        {
            if (float.TryParse(_gravityInputField.text, out float gravity))
            {
                Physics.gravity = new Vector3(0f, -gravity, 0f);
            }
        }

        private void OnJumpInputChanged(string jumpInput)
        {
            if (float.TryParse(_jumpInputField.text, out float jump))
            {
                movementScript.initialJumpForce = jump;
                movementScript.UpdateJumps();
            }
        }
    }
}

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
        [SerializeField] private TextMeshProUGUI jumpsRemainingText;
        [SerializeField] private TextMeshProUGUI nextJumpForceText;
        private string nextJumpForce;
        [SerializeField] private TextMeshProUGUI speedText;

        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private TMP_InputField _jumpInputField;
        [SerializeField] private TMP_InputField _jumpCountInputField;
        [SerializeField] private TMP_InputField _jumpDampeningFactorInputField;
        
        [Header("Controlled Values")]
        private float jumpMagnitude;

        private MovementExperimentation movementScript;
        private void Awake()
        {
            movementScript = GetComponent<MovementExperimentation>();
            
            _gravityInputField.onValueChanged.AddListener(OnGravityInputChanged);
            _jumpInputField.onValueChanged.AddListener(OnJumpForceInputChanged);
            _jumpCountInputField.onValueChanged.AddListener(OnJumpCountInputChanged);
            _jumpDampeningFactorInputField.onValueChanged.AddListener(OnJumpDampeningFactorInputChanged);
        }
        
        private void OnDestroy()
        {
            _gravityInputField.onValueChanged.RemoveListener(OnGravityInputChanged);
            _jumpInputField.onValueChanged.RemoveListener(OnJumpForceInputChanged);
            _jumpCountInputField.onValueChanged.RemoveListener(OnJumpCountInputChanged);
            _jumpDampeningFactorInputField.onValueChanged.RemoveListener(OnJumpDampeningFactorInputChanged);
        }

        private void Start()
        {
            _gravityInputField.text = Physics.gravity.magnitude.ToString();
            _jumpInputField.text = movementScript.initialJumpForce.ToString();
            _jumpCountInputField.text = movementScript.jumpCount.ToString();
            _jumpDampeningFactorInputField.text= movementScript.jumpDampeningFactor.ToString();
        }

        private void Update()
        {
            speedText.text = $"Flat Speed: {Math.Round(new Vector2(playerRigidbody.linearVelocity.z,playerRigidbody.linearVelocity.x).magnitude,3)}";
            jumpsRemainingText.text = $"Jumps Remaining: {movementScript._jumpsRemaining}";
            
            nextJumpForce = movementScript._jumpsRemaining > 0 ? movementScript._jumps[Math.Min(movementScript.jumpCount-movementScript._jumpsRemaining, movementScript._jumps.Count-1)].ToString() : "No Jumps Remaining";
            nextJumpForceText.text = $"Next Jump Force: {nextJumpForce}";
        }

        private void OnGravityInputChanged(string gravityInput)
        {
            if (float.TryParse(_gravityInputField.text, out float gravity))
            {
                Physics.gravity = new Vector3(0f, -gravity, 0f);
            }
        }

        private void OnJumpForceInputChanged(string jumpInput)
        {
            if (float.TryParse(_jumpInputField.text, out float jump))
            {
                movementScript.initialJumpForce = jump;
                movementScript.UpdateJumps();
            }
        }
        
        private void OnJumpCountInputChanged(string jumpCountInput)
        {
            if (int.TryParse(_jumpCountInputField.text, out int jumpCount))
            {
                movementScript.jumpCount = jumpCount;
                movementScript.UpdateJumps();
            }
        }
        
        private void OnJumpDampeningFactorInputChanged(string jumpDampeningFactorInput)
        {
            if (float.TryParse(_jumpDampeningFactorInputField.text, out float jumpDampeningFactor))
            {
                movementScript.jumpDampeningFactor = jumpDampeningFactor;
                movementScript.UpdateJumps();
            }
        }
    }
}

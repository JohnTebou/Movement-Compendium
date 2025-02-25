using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private TMP_InputField walkSpeedInputField;
        [SerializeField] private TMP_InputField sprintMultiplierInputField;
        
        [Space(5)]
        [SerializeField] private TMP_InputField groundDragInputField;
        [SerializeField] private TMP_InputField airDragInputField;
        [SerializeField] private TMP_InputField gravityInputField;
        [SerializeField] private TMP_InputField airControlInputField;
        
        [Space(5)]
        [SerializeField] private TMP_InputField jumpForceInputField;
        [SerializeField] private TMP_InputField jumpCountInputField;
        [SerializeField] private TMP_InputField jumpDampeningFactorInputField;
        
        [Header("Controlled Values")]
        private float jumpMagnitude;

        private MovementExperimentation movementScript;
        private void Awake()
        {
            movementScript = GetComponent<MovementExperimentation>();

            walkSpeedInputField.onValueChanged.AddListener(OnWalkSpeedInputChanged);
            sprintMultiplierInputField.onValueChanged.AddListener(OnSprintMultiplierInputChanged);
            
            groundDragInputField.onValueChanged.AddListener(OnGroundDragInputChanged);
            airDragInputField.onValueChanged.AddListener(OnAirDragInputChanged);
            gravityInputField.onValueChanged.AddListener(OnGravityInputChanged);
            airControlInputField.onValueChanged.AddListener(OnAirControlInputChanged);
            
            jumpForceInputField.onValueChanged.AddListener(OnJumpForceInputChanged);
            jumpCountInputField.onValueChanged.AddListener(OnJumpCountInputChanged);
            jumpDampeningFactorInputField.onValueChanged.AddListener(OnJumpDampeningFactorInputChanged);
        }
        
        private void OnDestroy()
        {
            walkSpeedInputField.onValueChanged.RemoveListener(OnWalkSpeedInputChanged);
            sprintMultiplierInputField.onValueChanged.RemoveListener(OnSprintMultiplierInputChanged);
            
            groundDragInputField.onValueChanged.RemoveListener(OnGroundDragInputChanged);
            airDragInputField.onValueChanged.RemoveListener(OnAirDragInputChanged);
            gravityInputField.onValueChanged.RemoveListener(OnGravityInputChanged);
            airControlInputField.onValueChanged.RemoveListener(OnAirControlInputChanged);
            
            jumpForceInputField.onValueChanged.RemoveListener(OnJumpForceInputChanged);
            jumpCountInputField.onValueChanged.RemoveListener(OnJumpCountInputChanged);
            jumpDampeningFactorInputField.onValueChanged.RemoveListener(OnJumpDampeningFactorInputChanged);
        }

        private void Start()
        {
            walkSpeedInputField.text = movementScript.walkSpeed.ToString();
            sprintMultiplierInputField.text = movementScript.sprintMultiplier.ToString();
            
            groundDragInputField.text = movementScript.groundDrag.ToString();
            airDragInputField.text = movementScript.airDrag.ToString();
            gravityInputField.text = Physics.gravity.magnitude.ToString();
            airControlInputField.text = movementScript.airControlMultiplier.ToString();
            
            jumpForceInputField.text = movementScript.initialJumpForce.ToString();
            jumpCountInputField.text = movementScript.jumpCount.ToString();
            jumpDampeningFactorInputField.text= movementScript.jumpDampeningFactor.ToString();
        }

        private void Update()
        {
            speedText.text = $"Flat Speed: {Math.Round(new Vector2(playerRigidbody.linearVelocity.z,playerRigidbody.linearVelocity.x).magnitude,3)}";
            jumpsRemainingText.text = $"Jumps Remaining: {movementScript._jumpsRemaining}";
            
            nextJumpForce = movementScript._jumpsRemaining > 0 ? movementScript._jumps[Math.Min(movementScript.jumpCount-movementScript._jumpsRemaining, movementScript._jumps.Count-1)].ToString() : "No Jumps Remaining";
            nextJumpForceText.text = $"Next Jump Force: {nextJumpForce}";
        }
        
        
        
        private void OnWalkSpeedInputChanged(string walkSpeedInput)
        {
            if (float.TryParse(this.walkSpeedInputField.text, out float walkSpeed))
            {
                movementScript.walkSpeed = walkSpeed;
            }
        }
        
        private void OnSprintMultiplierInputChanged(string sprintMultiplierInput)
        {
            if (float.TryParse(this.walkSpeedInputField.text, out float sprintMultiplier))
            {
                movementScript.sprintMultiplier = sprintMultiplier;
            }
        }
        
        private void OnGroundDragInputChanged(string groundDragInput)
        {
            if (float.TryParse(groundDragInputField.text, out float groundDrag))
            {
                movementScript.groundDrag = groundDrag;
            }
        }
        
        private void OnAirDragInputChanged(string airDragInput)
        {
            if (float.TryParse(airDragInputField.text, out float airDrag))
            {
                movementScript.airDrag = airDrag;
            }
        }
        
        private void OnAirControlInputChanged(string airControlInput)
        {
            if (float.TryParse(airControlInputField.text, out float airControl))
            {
                movementScript.airControlMultiplier = airControl;
            }
        }

        private void OnGravityInputChanged(string gravityInput)
        {
            if (float.TryParse(gravityInputField.text, out float gravity))
            {
                Physics.gravity = new Vector3(0f, -gravity, 0f);
            }
        }
        
        private void OnJumpForceInputChanged(string jumpInput)
        {
            if (float.TryParse(jumpForceInputField.text, out float jump))
            {
                movementScript.initialJumpForce = jump;
                movementScript.UpdateJumps();
            }
        }
        
        private void OnJumpCountInputChanged(string jumpCountInput)
        {
            if (int.TryParse(jumpCountInputField.text, out int jumpCount))
            {
                movementScript.jumpCount = jumpCount;
                movementScript.UpdateJumps();
            }
        }
        
        private void OnJumpDampeningFactorInputChanged(string jumpDampeningFactorInput)
        {
            if (float.TryParse(jumpDampeningFactorInputField.text, out float jumpDampeningFactor))
            {
                movementScript.jumpDampeningFactor = jumpDampeningFactor;
                movementScript.UpdateJumps();
            }
        }
    }
}

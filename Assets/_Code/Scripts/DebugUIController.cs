using System;
using TMPro;
using UnityEngine;

namespace _Code.Scripts
{
    public class DebugUIController: MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField _gravityInputField;
        [SerializeField] private TMP_InputField _jumpInputField;
        
        [Header("Controlled Values")]
        [SerializeField] private float gravityMagnitude;
        private float jumpMagnitude;

        private MovementExperimentation movementScript;
        private void Awake()
        {
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
            
        }

        private void Update()
        {
            
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
                jumpMagnitude = jump;
            }
        }
    }
}

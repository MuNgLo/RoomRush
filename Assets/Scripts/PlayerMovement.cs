using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        
        public Transform _cameraMount;
    public float _moveSpeed = 5.0f;
        public float Sensitivity = 1.0f, ySensMultiplier = 1.0f, yMinLimit = -85F, yMaxLimit = 85F;
        private float x = 0.0f;// Storing camera angle
        private float y = 0.0f;// Storing camera angle
    private bool _isJumping;
    private bool _isSprinting;
    private CharacterController _cc;

    private void Start()
        {
        _cc = GetComponent<CharacterController>();
        }
        // Update is called once per frame
        void Update()
        {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

            #region MOUSE INPUT
            // Get MouseInput
            x += Input.GetAxisRaw("Mouse X") * Sensitivity;                                              // Mouse horizontal input
            y -= Input.GetAxisRaw("Mouse Y") * (Sensitivity * ySensMultiplier);                          // Mouse vertical input multiplier needs to be halfed for symmetry
            // Apply changes to camera
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);                    // Clamping the max/min vertical angle
            if (_cameraMount)
            {
                _cameraMount.transform.localEulerAngles = new Vector3(y, 0, 0);   // Set the Cameras Y pitch
            }
        transform.localEulerAngles = new Vector3(0, x, 0);
        #endregion
        _isJumping = Input.GetButton("Jump");

        _cc.SimpleMove(InputVector() * _moveSpeed);

        }
        /// <summary>
        /// Returns a normalized vector
        /// </summary>
        /// <returns></returns>
        public Vector3 InputVector()
        {
            Vector3 vIn = Vector3.zero;

            if (Input.GetAxis("Horizontal") > 0.0f)
            {
                vIn.x += 1.0f;
            }
            if (Input.GetAxis("Horizontal") < 0.0f)
            {
                vIn.x -= 1.0f;
            }
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                vIn.z += 1.0f;
            }
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                vIn.z -= 1.0f;
            }
            return this.transform.TransformDirection(vIn).normalized;
        }
    }// End of PlayerInput


using DecaMovement.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandling : MonoBehaviour
    {
        public Transform playerView;     // Camera
        public float playerViewYOffset = 0.8f; // The height at which the camera is bound to
        public float xMouseSensitivity = 30.0f;
        public float yMouseSensitivity = 30.0f;
        // Camera rotations
        [SerializeField]
        private float rotX = 0.0f;
        [SerializeField]
        private float rotY = 0.0f;

        private Cmd _cmd = new Cmd() { };

        internal Cmd CMD { get => _cmd; private set => _cmd = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
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
            if(Core.Instance.PlayerState == PLAYERSTATE.DEAD) { return; }
            /* Camera rotation stuff, mouse controls this shit */
            rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
            rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

            // Clamp the X rotation
            if (rotX < -90)
                rotX = -90;
            else if (rotX > 90)
                rotX = 90;

            Core.Instance.Avatar.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
            playerView.rotation = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera
            _cmd.airRotation = false;
            _cmd.forwardMove = Input.GetAxisRaw("Vertical");
            _cmd.rightMove = Input.GetAxisRaw("Horizontal");
            _cmd.wishJump = Input.GetKey(KeyCode.Space);
            _cmd.crouch = false;

            // Set the camera's position to the transform
            playerView.position = new Vector3(
                Core.Instance.Avatar.position.x,
                Core.Instance.Avatar.position.y + playerViewYOffset,
                Core.Instance.Avatar.position.z);
        }

    internal void SetViewRotations(Quaternion rotation)
    {
        rotX = rotation.eulerAngles.x;
        rotY = rotation.eulerAngles.y;
    }
}// EOF CLASS


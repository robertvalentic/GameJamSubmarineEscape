using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is attached to the player game object and makes it move and rotate based on user input.
public class CharacterControl : MonoBehaviour
{
    public Transform _grapPointTransform = null;

    [SerializeField] private float _speed = 4.0f; // How fast the player character will move around
    [SerializeField] private float _gravity = 9.81f; // How fast the player character is pulled to the ground
    [SerializeField] private float _jumpSpeed = 5.0f; // How high the player character will jump
    [SerializeField] private GameObject _playerCamera = null; // Reference to the player characters camera component
    [SerializeField] private float _terminalVelocity = -10.0f;
    private CharacterController _controller; // Reference to the player characters CharacterController component
    private Vector3 _moveDir = Vector3.zero; // The desired direction the player wants to move
    private bool lookEnabled = true;
    

    // Start is called before the first frame update
    void Start()
    {
        // Gets the CharacterController component attached to this GameObject
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
            Move();
            Look();
    }

    // Moves the player character based on input
    public void Move()
    {
        _moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), _moveDir.y, Input.GetAxisRaw("Vertical")); // Sets the desired move direction based on players input

        // Transforms the desired movement direction into world space, so that forward will move the character along the controllers forward instead of the worlds
        _moveDir = transform.TransformVector(_moveDir); 
                                               
        if (!_controller.isGrounded) // Checks if the controller is on the ground
        {
            _moveDir.y -= _gravity * Time.deltaTime; // Applies a downward force to the controller if it is not grounded
        }
        else
        {
            // If the jump key is down then set the desired move direction vector to have a upward force
            if (Input.GetButtonDown("Jump"))
            {
                _moveDir.y = _jumpSpeed;
            }
            else
            {
                //_moveDir.y -= _gravity * Time.deltaTime; // Applies a downward force to the controller to make isGrounded work reliably
            }
        }

        if (_moveDir.y < _terminalVelocity)
        {
            _moveDir.y = _terminalVelocity;
        }

        // Moves the character controller (and jumps) based on the desired move direction, multiplied by desired speed, and delta time to make it framerate independant
        _controller.Move(_moveDir * Time.deltaTime * _speed);
    }


    // Rotates the character body and camera based on x and y input
    public void Look()
    {
        if (lookEnabled)
        {
            // Rotates the character body along it's y-axis
            transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X"));

            // Rotates the camera along it's x-axis
            _playerCamera.transform.Rotate(Vector3.left, Input.GetAxisRaw("Mouse Y"));
        }
    }

    public void DisableLook()
    {
        lookEnabled = false;
    }

    public void EnableLook()
    {
        lookEnabled = true;
    }

    public void AttachToGrabPoint(GameObject obj)
    {
        obj.transform.parent = _grapPointTransform;
        obj.transform.localPosition = Vector3.zero;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}

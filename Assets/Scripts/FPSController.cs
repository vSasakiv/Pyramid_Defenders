using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float acceleration = 30f;
    public float midAirAcceleration = 1f;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float fallSpeed = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float jumpBufferDuration = 0.1f;
    public float jumpEndEarlyModifier = 2f;
    public float slideSpeed = 5f;
    public bool jumpOnSlope = false;

    private Vector3 _moveSpeed = Vector3.zero;
    private float _rotationX;
    private Vector3 _currentSpeed = Vector3.zero;
    private float _jumpBufferCounter;
    private bool _jumpScheduled;
    private bool _isJumping;
    private float _moveSpeedY;

    private CharacterController _characterController;

    // Start is called before the first frame update
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        _HandlePlaneMovement();
        _HandleJump();
        _HandleSlopeSliding();
        _HandleCamera();
        _characterController.Move(_moveSpeed * Time.deltaTime);
    }

    private void _HandlePlaneMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float maxSpeed = isRunning ? runSpeed : walkSpeed;
        float currentAcceleration = _characterController.isGrounded ? acceleration : midAirAcceleration;

        float movingX = Input.GetAxisRaw("Vertical");
        float movingZ = Input.GetAxisRaw("Horizontal");

        // If the player is pressing move button, add acceleration 
        if (movingX > 0)
            _currentSpeed.x += currentAcceleration * Time.deltaTime;
        else if (movingX < 0)
            _currentSpeed.x -= currentAcceleration * Time.deltaTime;
        else
            _currentSpeed.x -= Mathf.Sign(_currentSpeed.x) * currentAcceleration * Time.deltaTime;

        if (movingZ > 0)
            _currentSpeed.z += currentAcceleration * Time.deltaTime;
        else if (movingZ < 0)
            _currentSpeed.z -= currentAcceleration * Time.deltaTime;
        else
            _currentSpeed.z -= Mathf.Sign(_currentSpeed.z) * currentAcceleration * Time.deltaTime;

        _currentSpeed.x = Mathf.Clamp(_currentSpeed.x, -maxSpeed, maxSpeed);
        _currentSpeed.z = Mathf.Clamp(_currentSpeed.z, -maxSpeed, maxSpeed);

        // saves vertical movement so it doesnt get zero'ed
        _moveSpeedY = _moveSpeed.y;
        
        _moveSpeed = (forward * _currentSpeed.x) + (right * _currentSpeed.z);

        _moveSpeed = Vector3.ClampMagnitude(_moveSpeed, maxSpeed);
    }

    private void _HandleJump()
    {
        // If jump button is pressed, schedule a jump to happen and zero the counter
        bool jumpPressed = Input.GetButton("Jump");
        // bool isGrounded = _isGrounded();

        // If the time passes and we still cannot jump, de-schedule the jump
        if (_jumpScheduled)
        {
            _jumpBufferCounter += Time.deltaTime;
            if (_jumpBufferCounter >= jumpBufferDuration)
                _jumpScheduled = false;
        }

        // If there's a scheduled jump and the character is grounded, jump
        if (_jumpScheduled && _characterController.isGrounded && !_isJumping)
        {
            _moveSpeed.y = jumpPower;
            _isJumping = true;
            _jumpScheduled = false;
        }
        else
        {
            _moveSpeed.y = _moveSpeedY;
        }
        
        if (jumpPressed)
        {
            _jumpScheduled = true;
            _jumpBufferCounter = 0f;
        }

        float currentFallSpeed = fallSpeed;

        // If it is still jumping, but the button is released
        if (_isJumping && !jumpPressed && _moveSpeed.y > 0)
            currentFallSpeed *= jumpEndEarlyModifier;

        _moveSpeed.y -= currentFallSpeed * Time.deltaTime;

        if (!_characterController.isGrounded) return;
        if (!(_moveSpeed.y > 0))
            _isJumping = false;
    }

    private void _HandleCamera()
    {
        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void _HandleSlopeSliding()
    {
        float sphereCastVerticalOffset = _characterController.height / 2 - _characterController.radius;
        Vector3 castOrigin = transform.position - new Vector3(0, sphereCastVerticalOffset, 0);

        if (!_characterController.isGrounded) return;
        
        // ray casts a sphere below the player, if it hits something, get the angle between the
        // normal of the surface it hit and the Y-axis, and apply a speed to simulate sliding
        if (!Physics.SphereCast(castOrigin,
                _characterController.radius - 0.01f,
                Vector3.down,
                out var hitInfo,
                1f,
                ~LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore))
            return;

        Vector3 normal = hitInfo.normal;
        float angle = Vector3.Angle(Vector3.up, normal);

        if (!(angle > _characterController.slopeLimit)) return;

        float yInverse = 1f - normal.y;
        _moveSpeed.x += yInverse * normal.x * slideSpeed;
        _moveSpeed.z += yInverse * normal.z * slideSpeed;

        if (jumpOnSlope) return;

        _jumpScheduled = false;
    }
}

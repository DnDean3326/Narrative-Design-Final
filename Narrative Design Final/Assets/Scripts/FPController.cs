using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class FPController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float acceleration = 15f;
    public float walkSpeed = 3.5f;
    public float sprintSpeed = 15f;
    public float crouchSpeed = 1f;
    public float jumpHeight = 2f;
    public bool canMultiJump = false;
    [Range(1, 5)] public int maxJumps = 1;
    public Vector3 normalScale = Vector3.one;
    public Vector3 crouchScale = new Vector3(1f, 0.75f, 1f);
    private int timesJumped = 0;
    private bool underSomething = false;
    private float MaxSpeed => crouchInput ? crouchSpeed : (sprintInput ? sprintSpeed : walkSpeed);
    private bool Sprinting { get { return sprintInput && CurrentSpeed > 0.1f; } }
    private bool CrouchWalking { get { return crouchInput && CurrentSpeed > 0.1f; } }

    [Header("Camera Properties")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 85f;
    private float currentPitch = 0f;
    private float CurrentPitch
    {
        get => currentPitch;
        set => currentPitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
    }
    public float cameraNormFOV = 60f;
    public float cameraSprintFOV = 80f;
    public float cameraCrouchFOV = 50f;
    public float cameraFOVSmoothing = 1f;

    [Header("Physics Properties")]
    public float gravityScale = 3f;
    private float verticalVelocity = 0f;
    private Vector3 CurrentVelocity { get; set; }
    private float CurrentSpeed { get; set; }
    private bool wasGrounded = false;
    private bool isGrounded => characterController.isGrounded;

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;
    public bool crouchInput;

    private CinemachineCamera fpCamera;
    private CharacterController characterController;

    [Header("Events")]
    public UnityEvent OnLanded;

    private void OnValidate()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if(fpCamera == null)
        {
            fpCamera = GetComponentInChildren<CinemachineCamera>();
        }
    }

    private void Update()
    {
        MoveUpdate();
        LookUpdate();
        CameraUpdate();

        //Triggers if grounded state changed from last frame
        if(!wasGrounded && isGrounded)
        {
            timesJumped = 0;
            OnLanded?.Invoke();
        }
        wasGrounded = isGrounded;
    }

    private void MoveUpdate()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0;
        motion.Normalize();

        //Movement smoothing
        if(motion.sqrMagnitude >= 0.01f)
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity,motion * MaxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }

        //Gravity
        if(isGrounded && verticalVelocity <= 0.01f)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }

        Vector3 fullVelocity = new Vector3(CurrentVelocity.x, verticalVelocity, CurrentVelocity.z);

        CollisionFlags flags = characterController.Move(fullVelocity * Time.deltaTime);
        if((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0.01f)
        {
            verticalVelocity = 0f;
        }

        underSomething = Physics.Raycast(new Ray(transform.Find("Head").position, transform.up), 5) || Physics.Raycast(new Ray(transform.Find("Head1").position, transform.up), 5);
        if (crouchInput || (!crouchInput && underSomething))
        {
            transform.localScale = crouchScale;
        }
        else
        {
            transform.localScale = normalScale;
        }

        CurrentSpeed = CurrentVelocity.magnitude;
    }

    private void LookUpdate()
    {
        Vector2 input = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);

        //Vertical
        CurrentPitch -= input.y;
        fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        //Horizontal
        transform.Rotate(Vector3.up * input.x);
    }

    private void CameraUpdate()
    {
        //Camera FOV smoothing
        float targetFOV = cameraNormFOV;
        if(Sprinting)
        {
            targetFOV = Mathf.Lerp(cameraNormFOV, cameraSprintFOV, CurrentSpeed / sprintSpeed);
        }
        else if (CrouchWalking)
        {
            targetFOV = Mathf.Lerp(cameraNormFOV, cameraCrouchFOV, crouchSpeed / CurrentSpeed);
        }

        fpCamera.Lens.FieldOfView = Mathf.Lerp(fpCamera.Lens.FieldOfView, targetFOV, cameraFOVSmoothing * Time.deltaTime);
    }

    public void TryJump()
    {
        if (!isGrounded)
        {
            if(canMultiJump && timesJumped < maxJumps && verticalVelocity > 0.01f)
            {
                return;
            }

            if(!canMultiJump || timesJumped >= maxJumps)
            {
                return;
            }

        }

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
        timesJumped++;
    }

    public void TryCrouch()
    {

    }
}

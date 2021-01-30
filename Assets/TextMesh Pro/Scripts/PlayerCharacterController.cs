using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler), typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public AudioSource audioSource;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;
    PlayerInputHandler m_InputHandler;
    public float rotationSpeed = 200f;
    public float RotationMultiplier = 0.4f;
    float m_CameraVerticalAngle = 0f;

    public float footstepSFXFrequencyWhileSprinting = 1f;
    public AudioClip footstepSFX;
    public AudioClip jumpSFX;
    public AudioClip landSFX;

    float m_footstepDistanceCounter;
    public float footstepSFXFrequency = 1f;
    public bool isGrounded { get; private set; }
const float k_JumpGroundingPreventionTime = 0.2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = characterController.isGrounded;

        if (isGrounded && !wasGrounded)
        {
            audioSource.PlayOneShot(landSFX);
        }

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = m_InputHandler.GetSprintInputHeld();
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * m_InputHandler.GetMoveInput().z : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * m_InputHandler.GetMoveInput().x : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (m_InputHandler.GetJumpInputDown() && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (isRunning)
        {
            EventManager.TriggerEvent("NOISE", new Hashtable() { { "RUN", 0.05f } });
        }

        if(characterController.velocity.magnitude < 5)
        {
            EventManager.TriggerEvent("SILENCE", null);
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // footsteps sound
        float chosenFootstepSFXFrequency = (isRunning ? footstepSFXFrequencyWhileSprinting : footstepSFXFrequency);
        if (m_footstepDistanceCounter >= 1f / chosenFootstepSFXFrequency && characterController.isGrounded)
        {
            m_footstepDistanceCounter = 0f;
            audioSource.PlayOneShot(footstepSFX);
        }
        m_footstepDistanceCounter += characterController.velocity.magnitude * Time.deltaTime;


        // Player and Camera rotation
        if (canMove)
        {
                // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * rotationSpeed * RotationMultiplier), 0f), Space.Self);

            // vertical camera rotation
            {
                // add vertical inputs to the camera's vertical angle
                m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * rotationSpeed * RotationMultiplier;

                // limit the camera's vertical angle to min/max
                m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

                // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
                playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Interactable")
        {
            EventManager.TriggerEvent("NOISE", new Hashtable() { { "COLLIDE_PLAYER", 10F } });
        }
    }

}

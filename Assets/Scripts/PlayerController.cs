using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpFactor = 5f;
    public float distToGround = 1f;

    public bool jumping;
    public bool moving;

    private Rigidbody rig;
    private Animator anim;

    float m_CameraVerticalAngle = 0f;

    public float rotationSpeed = 200f;
    public float RotationMultiplier = 0.4f;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (Input.GetAxisRaw("Mouse X") * 1 * 0.05f * rotationSpeed * RotationMultiplier), 0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += Input.GetAxisRaw("Mouse Y") * 1 * 0.05f * rotationSpeed * RotationMultiplier;

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            Camera.main.transform.localEulerAngles = new Vector3(-m_CameraVerticalAngle, 0, 0);
        }

        if (direction.magnitude > 0)
        {
            Vector2 movement = direction * speed * Time.deltaTime;

            transform.Translate(movement.y, 0, movement.x, Space.Self);
            moving = true;
        }
        else moving = false;

        jumping = !IsGrounded();

        anim.SetFloat("Horizontal", direction.y);
        anim.SetFloat("Vertical", direction.x);
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            rig.AddForce(new Vector3(0, jumpFactor, 0), ForceMode.Impulse);
            EventManager.TriggerEvent("NOISE", new Hashtable() { { "JUMP", 20 } });
        }

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}

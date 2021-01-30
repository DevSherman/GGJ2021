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
        }

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
}

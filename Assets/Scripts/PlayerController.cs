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

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (v > 0 || h > 0)
        {
            transform.Translate(h, 0, v, Space.Self);
            moving = true;
        }
        else moving = false;

        jumping = !IsGrounded();
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

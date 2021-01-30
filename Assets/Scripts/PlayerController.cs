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
    private Camera mainCamera;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        mainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (v != 0 || h != 0)
        {
            transform.Translate(h, 0, v, Space.Self);
            moving = true;
        }
        else moving = false;

        jumping = !IsGrounded();

        //Wawla();
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

    /*void Wawla()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Linecast(mainCamera.transform.position, mainCamera.transform.forward * 100, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);    
        }
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 100, Color.red);
    }*/
}

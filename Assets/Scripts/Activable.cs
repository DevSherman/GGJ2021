using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    private bool active;

    public float pos_speed = 5f;
    private Vector3 pos_origin;
    public Vector3 pos_end;

    public bool onlyRotate;
    public float rot_speed = 5f;
    private Quaternion rot_origin;
    public Quaternion rot_end;

    private void Start()
    {
        pos_origin = transform.localPosition;
        rot_origin = transform.localRotation;
    }

    public void Use()
    {
        active = !active;
    }

    public void Update()
    {
        if(!onlyRotate)
        {
            if (active) transform.localPosition = Vector3.Lerp(transform.localPosition, pos_end, Time.deltaTime * pos_speed);
            else transform.localPosition = Vector3.Lerp(transform.localPosition, pos_origin, Time.deltaTime * pos_speed);
        }
        else
        {
            if (active) transform.localRotation = Quaternion.Lerp(transform.localRotation, rot_end, Time.deltaTime * rot_speed);
            else transform.localRotation = Quaternion.Lerp(transform.localRotation, rot_origin, Time.deltaTime * rot_speed);
        }
    }
}

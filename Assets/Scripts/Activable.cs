using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour
{
    private Animator anim;
    private bool active;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Use()
    {
        active = !active;
        anim.SetBool("active", active);
    }
}

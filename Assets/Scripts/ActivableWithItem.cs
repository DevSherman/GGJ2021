using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableWithItem : MonoBehaviour
{
    public string itemNameRequiered;

    public void Use()
    {
        GetComponent<Activable>().Use();
    }
}

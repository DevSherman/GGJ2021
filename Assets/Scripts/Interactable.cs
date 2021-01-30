using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isPickable = true;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "PlayerInteractionZone") 
        {
            other.GetComponent<Interactor>().objectToInteract = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "PlayerInteractionZone")
        {
            other.GetComponent<Interactor>().objectToInteract = null;
        }
    }
}

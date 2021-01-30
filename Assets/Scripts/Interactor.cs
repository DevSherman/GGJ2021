using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject currentInteractable;
    public GameObject objectToInteract;
    public Transform interactionZone;
    private Camera mainCamera;
    public bool grabbing;


    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!grabbing)
        {
            Drag();
        }
        else
        {
            Drop();
        }
    }

    void Drag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Linecast(mainCamera.transform.position, mainCamera.transform.forward * 100, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider != null && hit.collider.CompareTag("Interactable"))
            {
                if (hit.collider.GetComponent<Interactable>().isPickable)
                {
                    objectToInteract = hit.collider.gameObject;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        currentInteractable = objectToInteract;
                        objectToInteract = null;
                        currentInteractable.GetComponent<Interactable>().isPickable = false;
                        currentInteractable.transform.SetParent(interactionZone.transform);
                        currentInteractable.transform.position = interactionZone.position;
                        currentInteractable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        currentInteractable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        grabbing = true;

                    }


                }

            }

            

        }
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 100, Color.red);
    } 

    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.GetComponent<Interactable>().isPickable = true;
            currentInteractable.transform.SetParent(null);
            currentInteractable.gameObject.GetComponent<Rigidbody>().useGravity = true;
            currentInteractable.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            currentInteractable = null;
            grabbing = false;
        }
    }
}

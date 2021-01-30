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
    public float distance = 5f;
    public float smooth = 5f;

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
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out hit))
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
                        currentInteractable.transform.SetParent(mainCamera.transform);
                        currentInteractable.transform.position = Vector3.Lerp(
                            currentInteractable.transform.position,
                            mainCamera.transform.position + mainCamera.transform.forward * distance,
                            Time.deltaTime * smooth);
                        currentInteractable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        currentInteractable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        grabbing = true;

                    }


                }

            }

            

        }
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

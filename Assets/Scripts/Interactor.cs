using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public GameObject currentInteractable;
    public GameObject objectToInteract;
    public Transform interactionZone;
    private Camera mainCamera;
    public bool grabbing;
    public float distance = 5f;
    public float smooth = 5f;

    public Image itemImage;
    public List<Item> items;


    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        items = new List<Item>();
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
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)), out hit, distance))
        {
            //Debug.Log(hit.collider.gameObject.name);

            if (objectToInteract != null && hit.collider.gameObject != objectToInteract)
            {
                objectToInteract.GetComponent<Outline>().HideOutline();
            }

            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);

                if (hit.collider.CompareTag("Interactable"))
                {
                    if (hit.collider.GetComponent<Interactable>().isPickable)
                    {
                        objectToInteract = hit.collider.gameObject;
                        objectToInteract.GetComponent<Outline>().ShowOutline();

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            currentInteractable = objectToInteract;
                            //objectToInteract = null;
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
                if (hit.collider.CompareTag("Item"))
                {
                    objectToInteract = hit.collider.gameObject;
                    objectToInteract.GetComponent<Outline>().ShowOutline();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        AddItemToInv(objectToInteract.GetComponent<Item>());
                    }
                }
            }
            else 
            {
                HideOutline();
            }
        }
        else
        {
            HideOutline();
        }
    }
    void HideOutline()
    {
        if (objectToInteract != null)
            objectToInteract.GetComponent<Outline>().HideOutline();
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

    void AddItemToInv(Item item)
    {
        items.Add(item);
        Destroy(objectToInteract);
        objectToInteract = null;
    }
}

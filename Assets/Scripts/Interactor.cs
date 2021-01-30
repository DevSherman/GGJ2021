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
    public Sprite itemNullSprite;
    private List<Item> items;
    private Item currentItem;
    private int itemIndex = -1;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        items = new List<Item>();
    }

    private void Update()
    {
        //Item Scroll
        if(items.Count > 0)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                itemIndex++;
                if (itemIndex > items.Count - 1) itemIndex = 0;

                ItemSelectedUI();
            }
        }

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
                        AddItemToInv();
                    }
                }
                if (hit.collider.CompareTag("Activable"))
                {
                    objectToInteract = hit.collider.gameObject;
                    objectToInteract.GetComponent<Outline>().ShowOutline();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        objectToInteract.GetComponent<Activable>().Use();
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

    void AddItemToInv()
    {
        Item item = objectToInteract.GetComponent<Item>();
        items.Add(item);

        if (itemImage.sprite == itemNullSprite)
        {
            itemImage.sprite = item.itemImage;
            currentItem = item;
        }

        Destroy(objectToInteract);
        objectToInteract = null;
    }

    void ItemSelectedUI()
    {
        currentItem = items[itemIndex];
        itemImage.sprite = currentItem.itemImage;
    }

    void UseItem()
    {
        if(currentItem != null)
        {
            items.Remove(currentItem);
            if(items.Count > 0)
            {
                itemIndex--;
                ItemSelectedUI();
            }
            if(items.Count == 0)
            {
                itemIndex = -1;
                itemImage.sprite = itemNullSprite;
            }

        }
    }
}

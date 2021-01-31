using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public GameObject currentInteractable;
    public GameObject objectToInteract;
    //public Transform interactionZone;
    private Camera mainCamera;
    public bool grabbing;
    public float distance = 5f;
    public float smooth = 5f;

    public Image itemImage;
    public Sprite itemNullSprite;
    private List<Item> items;
    public Item currentItem;
    public int itemIndex = -1;
    public ObjectiveManager objectiveManager;


    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        items = new List<Item>();

        itemNullSprite = Resources.Load<Sprite>("itemNull");
        itemImage = GameObject.Find("GameHUD").GetComponent<GameHUD>().itemImage;

    }

    private void Update()
    {
        //Item Scroll

        if(Input.GetKeyDown(KeyCode.Q) && items.Count > 0)
        {
            itemIndex++;
            if (itemIndex > items.Count - 1) itemIndex = 0;

            ItemSelectedUI();
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

                /*if (hit.collider.CompareTag("Objective"))
                {
               

                    objectToInteract = hit.collider.gameObject;
                    objectToInteract.GetComponent<Outline>().ShowOutline();
                    var itemName = objectToInteract.GetComponent<Item>().itemName;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        AddObjective(itemName);
                    }
                }*/

                if (hit.collider.CompareTag("Activable"))
                {
                    objectToInteract = hit.collider.gameObject;
                    objectToInteract.GetComponent<Outline>().ShowOutline();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        objectToInteract.GetComponent<Activable>().Use();
                    }
                }
                if (hit.collider.CompareTag("ActivableWithItem"))
                {
                    objectToInteract = hit.collider.gameObject;
                    objectToInteract.GetComponent<Outline>().ShowOutline();

                    if (Input.GetKeyDown(KeyCode.E)) UseItem();
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

        EventManager.TriggerEvent("OBJECTIVE", new Hashtable() { { item.itemName.ToUpper(), null } });

        if (itemImage.sprite == itemNullSprite)
        {
            itemImage.sprite = item.itemImage;
            currentItem = item;
            itemIndex++;
        }

        item.gameObject.GetComponent<MeshRenderer>().enabled = false;
        item.gameObject.GetComponent<Collider>().enabled = false;

        objectToInteract = null;
    }

    /*void AddObjective(string name)
    {
        Item item = objectToInteract.GetComponent<Item>();
        objectiveManager.m_Objectives.Add(item);
        item.gameObject.GetComponent<MeshRenderer>().enabled = false;
        item.gameObject.GetComponent<Collider>().enabled = false;
        item.gameObject.GetComponent<Item>().isPicked = true;
        objectToInteract = null;

        EventManager.TriggerEvent("OBJECTIVE", new Hashtable() { { name, null } });

    }*/

    void ItemSelectedUI()
    {
        currentItem = items[itemIndex];
        itemImage.sprite = currentItem.itemImage;
    }

    void UseItem()
    {
        if (currentItem != null)
        {
            ActivableWithItem script = objectToInteract.GetComponent<ActivableWithItem>();
            if (script.itemNameRequiered == currentItem.itemName)
            {
                script.Use();

                items.Remove(currentItem);
                Destroy(currentItem);
                currentItem = null;

                if(items.Count == 1)
                {
                    currentItem = items[0];
                    itemImage.sprite = currentItem.itemImage;
                    return;
                }

                if (items.Count > 1)
                {
                    itemIndex--;
                    ItemSelectedUI();
                    return;
                }
                if (items.Count == 0)
                {
                    itemIndex = -1;
                    itemImage.sprite = itemNullSprite;
                    return;
                }

            }
        }
    }
}

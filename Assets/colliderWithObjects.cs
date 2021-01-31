using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderWithObjects : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip landSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Interactable")
        {
            audioSource.PlayOneShot(landSFX);
            EventManager.TriggerEvent("NOISE", new Hashtable() { { "COLLIDE_WITH_PLAYER", 10f } });
        }
    }
}

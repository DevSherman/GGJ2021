using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzIntermitente : MonoBehaviour
{
    Light luzparpadea;
    public float minEspera;
    public float maxEspera;

    private void Start()
    {
        luzparpadea = GetComponent<Light>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minEspera, maxEspera));
            luzparpadea.enabled = !luzparpadea.enabled;
        }
    }
}

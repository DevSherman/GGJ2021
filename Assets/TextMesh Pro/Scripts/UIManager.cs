using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image noiseBar;
    public float MaxValueNoiseBar = 100;
    public float currentValue = 0;

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
       
    }

    public void OnEnable()
    {
        EventManager.StartListening("UPDATE_NOISE_BAR", HandleNoiseBarFloat);

    }

    public void OnDisable()
    {
        EventManager.StopListening("UPDATE_NOISE_BAR", HandleNoiseBarFloat);

    }

    void HandleNoiseBar(Hashtable eventParams)
    {
        currentValue = int.Parse(eventParams["NOISE_VALUE"].ToString());

        noiseBar.fillAmount = currentValue / MaxValueNoiseBar;

    }

    void HandleNoiseBarFloat(Hashtable eventParams)
    {
         currentValue = float.Parse(eventParams["NOISE_VALUE"].ToString());

        noiseBar.fillAmount = currentValue / MaxValueNoiseBar;
    }
}

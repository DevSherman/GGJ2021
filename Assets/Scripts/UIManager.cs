using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image noiseBar;
    public float MaxValueNoiseBar = 100;

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
       
    }

    public void OnEnable()
    {
        EventManager.StartListening("UPDATE_NOISE_BAR", HandleNoiseBar);
    }

    public void OnDisable()
    {
        EventManager.StopListening("UPDATE_NOISE_BAR", HandleNoiseBar);
    }

    void HandleNoiseBar(Hashtable eventParams)
    {
        int currentValue = int.Parse(eventParams["NOISE_VALUE"].ToString());

        noiseBar.fillAmount = currentValue / MaxValueNoiseBar;
    }
}

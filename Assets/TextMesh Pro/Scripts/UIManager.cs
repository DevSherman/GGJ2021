using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image noiseBar;
    public float MaxValueNoiseBar = 100;
    public float currentValue = 0;

    public Toggle checkJoystick;
    public Toggle checkGame;
    public Toggle checkConsole;
    public Toggle checkWire;

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
        EventManager.StartListening("CHECK", HandleCheck);
    }

    private void HandleCheck(Hashtable eventParams)
    {
        if (eventParams.ContainsKey("JOYSTICK"))
        {

            checkJoystick.isOn = true;
        }
        if (eventParams.ContainsKey("GAME"))
        {
            checkGame.isOn = true;
        }
        if (eventParams.ContainsKey("CONSOLE"))
        {
            checkConsole.isOn = true;
        }
        if (eventParams.ContainsKey("WIRE"))
        {
            checkWire.isOn = true;
        }
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

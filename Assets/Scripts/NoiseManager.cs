using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public float noise = 0;

    public void OnEnable()
    {
        EventManager.StartListening("NOISE", HandleNoise);
    }

    public void OnDisable()
    {
        EventManager.StopListening("NOISE", HandleNoise);
    }

    public void HandleNoise(Hashtable eventParams)
    {
        if (noise >= 100F)
        {
            EventManager.TriggerEvent("GAME_OVER");
        }

        if (eventParams.ContainsKey("FALL"))
        {
            float noiseIncoming = float.Parse(eventParams["FALL"].ToString());
            
            AddNoise(noiseIncoming);
        }

        if (eventParams.ContainsKey("RUN"))
        {
            float noiseIncoming = 0.05f;

            AddNoise(noiseIncoming);
        }
    } 

    public void AddNoise(float noiseIncoming)
    {
      
        noise += noiseIncoming;
        EventManager.TriggerEvent("UPDATE_NOISE_BAR", new Hashtable() { { "NOISE_VALUE", noise } });
     
    }

    public void ReduceNoise(float value)
    {
        noise -= value;
        EventManager.TriggerEvent("UPDATE_NOISE_BAR", new Hashtable() { { "NOISE_VALUE", noise } });

    }

}

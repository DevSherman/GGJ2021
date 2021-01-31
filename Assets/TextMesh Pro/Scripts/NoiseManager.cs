using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public float noise = 0;

    public void OnEnable()
    {
        EventManager.StartListening("NOISE", HandleNoise);
        EventManager.StartListening("SILENCE", HandleSilence);

    }

    public void OnDisable()
    {
        EventManager.StopListening("NOISE", HandleNoise);
        EventManager.StopListening("SILENCE", HandleSilence);

    }

    private void Update()
    {
       
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
            float noiseIncoming = 1;

            AddNoise(noiseIncoming);
        }

        if (eventParams.ContainsKey("COLLIDE_WITH_PLAYER"))
        {
            float noiseIncoming = float.Parse(eventParams["COLLIDE_WITH_PLAYER"].ToString());

            AddNoise(noiseIncoming);
        }
    }

    public void HandleSilence(Hashtable eventParams)
    {
        ReduceNoise(0.005f);
    }

    public void AddNoise(float noiseIncoming)
    {
        if (noise >= 100) return;

        noise += noiseIncoming;
        EventManager.TriggerEvent("UPDATE_NOISE_BAR", new Hashtable() { { "NOISE_VALUE", noise } });
     
    }

    public void ReduceNoise(float value)
    {
        if (noise <= 0) return;

        noise -= value;
        EventManager.TriggerEvent("UPDATE_NOISE_BAR", new Hashtable() { { "NOISE_VALUE", noise } });

    }


}

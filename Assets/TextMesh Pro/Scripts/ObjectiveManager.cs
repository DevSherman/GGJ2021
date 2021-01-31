using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening("OBJECTIVE", HandleObjective);
    }
    public int index = 0;

    private void HandleObjective(Hashtable eventParams)
    {
        if (index == 3)
        {
            Debug.Log("win");
            EventManager.TriggerEvent("WIN_GAME");
        }

        if (eventParams.ContainsKey("JOYSTICK"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "JOYSTICK", null } });
            index++;
        }
        if (eventParams.ContainsKey("CONSOLE"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "CONSOLE", null } });
            index++;
        }
        if (eventParams.ContainsKey("WIRES"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "WIRES", null } });
            index++;
        }
        if (eventParams.ContainsKey("GAME"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "GAME", null } });
            index++;
        }

    }

    private void OnDisable()
    {
        EventManager.StopListening("OBJECTIVE", HandleObjective);
    }
}

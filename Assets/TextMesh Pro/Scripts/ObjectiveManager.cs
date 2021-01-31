using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<Item> m_Objectives = new List<Item>();
    public int numberOfObjectives;
    public bool AreAllObjectivesCompleted()
    {

        if (m_Objectives.Count == 0)
            return false;

        for (int i = 0; i < m_Objectives.Count; i++)
        {
            // pass every objectives to check if they have been completed
            if (m_Objectives[i].isPicked && m_Objectives.Count >= numberOfObjectives)
            {
                return true;
            }
        }

        return false;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OBJECTIVE", HandleObjective);
    }

    private void HandleObjective(Hashtable eventParams)
    {
        if (AreAllObjectivesCompleted())
        {

            Debug.Log("win");
            EventManager.TriggerEvent("WIN_GAME");
        }

        if (eventParams.ContainsKey("JOYSTICK"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "JOYSTICK", null } });
        }
        if (eventParams.ContainsKey("CONSOLE"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "CONSOLE", null } });
        }
        if (eventParams.ContainsKey("WIRES"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "WIRES", null } });
        }
        if (eventParams.ContainsKey("GAME"))
        {
            EventManager.TriggerEvent("CHECK", new Hashtable() { { "GAME", null } });
        }

    }

    private void OnDisable()
    {
        EventManager.StopListening("OBJECTIVE", HandleObjective);
    }
}

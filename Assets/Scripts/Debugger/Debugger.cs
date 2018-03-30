using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {

    private static Debugger s_Instance = null;

    enum DEBUG
    {
        _DBG_PLAYER_MOVEMENT = 0,
        _DBG_PLAYER_HEALTH,
        _DBG_COUNT
    }

    public bool[] _flags = flags;
    public static bool[] flags = new bool[(int)DEBUG._DBG_COUNT];

    public static Debugger instance
    {
        get
        {
            if(s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(Debugger)) as Debugger;
            }

            //if no active instance, create one
            if(s_Instance == null)
            {
                GameObject obj = new GameObject("Debugger");
                s_Instance = obj.AddComponent(typeof(Debugger)) as Debugger;
                Debug.Log("[Debugger:] Couldn't localte a Debugger object. Debugger was created automatically.");
            }

            return s_Instance;
        }
    }

    private void OnApplicationQuit()
    {
        s_Instance = null;
    }

    public static void DebugMessage(string module, string message)
    {
        if(canPrint(getModuleID(module)))
            Debug.Log("[" + module + "] => " + message);
    }

    public void Toggle(int index)
    {
        flags[index] = !flags[index];
    }

    private static int getModuleID(string module)
    {
        switch(module)
        {
            case "PlayerMovement":
                return (int)DEBUG._DBG_PLAYER_MOVEMENT;
            case "PlayerHealth":
                return (int)DEBUG._DBG_PLAYER_HEALTH;
            default:
                return -1;
        }
    }
    
    public static bool canPrint(int index)
    {
        return (flags[index] == true);
    }

    private void Update()
    {
        //update acces/update debug from editor
        if (Input.GetKeyDown(KeyCode.Escape))
            _flags = flags;
    }
}

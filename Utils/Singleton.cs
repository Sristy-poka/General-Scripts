using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<variableT> : MonoBehaviour where variableT : Singleton<variableT>
{
    private static variableT instance;
    public static variableT Instance
    {
        get { return instance; }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("[singleton] trying to instantiate a second instance of a singleton class");
        }
        else
        {
            instance = (variableT)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}

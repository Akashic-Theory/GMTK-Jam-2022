using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRedirect : MonoBehaviour
{
    public string[] eventNames;

    public Dictionary<string, Action> actions { get; private set; } = new Dictionary<string, Action>();

    private void Awake()
    {
        foreach(string name in eventNames)
        {
            actions.Add(name, new Action(() => { }));
        }
    }

    public void Add(string name, Action action)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogWarning("GetAction looking for invalid name: '" + name + "'");
            return;
        }

        actions[name] += action;
    }

    public void Dispatch(string name)
    {
        if(!actions.ContainsKey(name))
        {
            Debug.LogError("Dispatch looking for invalid name: '" + name + "'");
            return;
        }

        actions[name]();
    }
}

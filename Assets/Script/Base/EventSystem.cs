using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, Event> events = new Dictionary<string, Event>();

    public void SubscribeEvent(string eventName, Action action)
    {
        if (!events.ContainsKey(eventName)) 
        {
            events.Add(eventName, new Event());
        }
        events[eventName].actions += action;
    }

    //取消订阅
    public void UnSubscribeEvent(string eventName, Action action)
    {
        if (!events.ContainsKey(eventName))
        {
            //Debug.log("Cannot find event");
            return;
        }
        else if (true)
        {
            events[eventName].actions -= action;
        }
    }

    public void SendEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
        {
            if (events[eventName].actions == null) 
            {
                //Debug.logError("Action null");
                return;
            }
            events[eventName].actions.Invoke();
        }
        else
        {
            //Debug.log("No Subscriber On Event:" + eventName);
            return;
        }
    }

    public void GetEventNames()
    {
        foreach (string name in events.Keys)
        {
            //Debug.log(name);
        }
    }

}
public class Event
{
    public Action actions;
}

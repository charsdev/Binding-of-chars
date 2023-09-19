using System;
using System.Collections.Generic;
using UnityEngine;

public class SDKEventargs : Dictionary<string, object> { }

public class SDKBoyEvent
{
    private event Action<SDKEventargs> eventListeners;

    public void AddListener(Action<SDKEventargs> listener)
    {
        eventListeners += listener;
    }

    public void RemoveListener(Action<SDKEventargs> listener)
    {
        eventListeners -= listener;
    }

    public void Invoke(SDKEventargs eventArgs)
    {
        eventListeners?.Invoke(eventArgs);
    }
}

public class SDKBoyEventManager : MonoBehaviour
{
    private Dictionary<string, SDKBoyEvent> eventDictionary;

    private static SDKBoyEventManager eventManager;

    public static SDKBoyEventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(SDKBoyEventManager)) as SDKBoyEventManager;

                if (!eventManager)
                {
                    eventManager = new GameObject("EventBus").AddComponent<SDKBoyEventManager>();
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, SDKBoyEvent>();
        }
    }

    public static void StartListening(string eventName, Action<SDKEventargs> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out SDKBoyEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new SDKBoyEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<SDKEventargs> listener)
    {
        if (eventManager == null) return;

        if (Instance.eventDictionary.TryGetValue(eventName, out SDKBoyEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, SDKEventargs eventParams = null)
    {
        if (Instance.eventDictionary.TryGetValue(eventName, out SDKBoyEvent thisEvent))
        {
            thisEvent.Invoke(eventParams);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        TriggerEvent(eventName, null);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent
{
    PAUSE,
    START
}

public interface IGameEventArgs { }

public class PropertyChangeArg<T> : IGameEventArgs {
       public T value;
}

public class EventBus : MonoBehaviour
{
    private static EventBus instance;

    public static EventBus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventBus>();

                if (instance == null)
                {
                    instance = new GameObject("EventBus").AddComponent<EventBus>();
                }
            }
            return instance;
        }
    }

    private Dictionary<string, Delegate> _eventDictionary = new Dictionary<string, Delegate>();

    public void Subscribe<T>(GameEvent eventEnum, Action<T> listener) where T : IGameEventArgs
    {
        Subscribe(eventEnum.ToString(), listener);
    }

    public void UnSuscribe<T>(GameEvent eventEnum, Action<T> listener) where T : IGameEventArgs
    {
        UnSuscribe(eventEnum.ToString(), listener);
    }

    public void Subscribe<T>(string eventEnum, Action<T> listener) where T : IGameEventArgs
    {
        if (!_eventDictionary.ContainsKey(eventEnum))
        {
            _eventDictionary[eventEnum] = null;
        }
        _eventDictionary[eventEnum] = (Action<T>)_eventDictionary[eventEnum] + listener;
    }

    public void UnSuscribe<T>(string eventEnum, Action<T> listener) where T : IGameEventArgs
    {
        if (!_eventDictionary.ContainsKey(eventEnum))
        {
            _eventDictionary[eventEnum] = null;
        }
        _eventDictionary[eventEnum] = (Action<T>)_eventDictionary[eventEnum] - listener;
    }

    public void TriggerEvent<T>(string eventEnum, T eventArgs) where T : IGameEventArgs
    {
        if (_eventDictionary.TryGetValue(eventEnum, out Delegate thisEvent))
        {
            (thisEvent as Action<T>)?.Invoke(eventArgs);
        }
    }

    public void TriggerEvent<T>(GameEvent eventEnum, T eventArgs) where T : IGameEventArgs
    {
        TriggerEvent(eventEnum, eventArgs);
    }

}



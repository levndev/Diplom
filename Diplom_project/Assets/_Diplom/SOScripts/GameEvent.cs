using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = MenuNames.GameEvent + "GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new();

    public void Raise()
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] == null)
                listeners.RemoveAt(i);
            else
                listeners[i].OnEventRaised();
        }
    }

    public void AddListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}

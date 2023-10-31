using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventResponse : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> response;

    public void Raise(GameObject sender)
    {
        response?.Invoke(sender);
    }
}

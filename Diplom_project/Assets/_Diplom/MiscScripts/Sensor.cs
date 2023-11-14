using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    [SerializeField] public UnityEvent OnTriggered;
    [SerializeField] private string filterTag;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(filterTag))
            OnTriggered?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(filterTag))
            OnTriggered?.Invoke();
    }
}

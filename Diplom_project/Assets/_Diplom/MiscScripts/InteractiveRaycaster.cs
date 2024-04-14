using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveRaycaster : MonoBehaviour
{
    [SerializeField] private UnityEvent<string> onInteractiveAcquired;
    [SerializeField] private UnityEvent onInteractiveLost;
    [SerializeField] private float raycastRange;
    [SerializeField] private string targetTagName;
    private Interactive currentInteractive = null;

    // Update is called once per frame
    void Update()
    {
        bool hitInteractive = false;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastRange))
        {
            if (hit.collider.gameObject.CompareTag(targetTagName))
            {
                hitInteractive = true;
                currentInteractive = hit.collider.gameObject.GetComponent<Interactive>();
                onInteractiveAcquired?.Invoke(currentInteractive.GetPrompt());
            }
        }
        if (!hitInteractive)
        {
            if (currentInteractive != null)
            {
                currentInteractive = null;
                onInteractiveLost?.Invoke();
            }
        }
    }

    public void TryInteract()
    {
        if (currentInteractive != null)
        {
            currentInteractive.onInteract?.Invoke();
        }
    }
}

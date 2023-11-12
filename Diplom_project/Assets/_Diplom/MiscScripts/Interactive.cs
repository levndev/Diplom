using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactive : MonoBehaviour
{
    [SerializeField] public UnityEvent onInteract;
    [SerializeField] private string interactPrompt;

    public string GetPrompt()
    {
        return interactPrompt;
    }
}

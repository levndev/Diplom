using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private MonoBehaviour playerLook;

    public void ToggleFlashlight()
    {
        if (flashlight != null)
            flashlight.enabled = !flashlight.enabled;
    }

    public void ToggleFreeze()
    {
        if (playerMovement != null)
            playerMovement.enabled = !playerMovement.enabled;
        if (playerLook != null)
            playerLook.enabled = !playerLook.enabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private PlayerKBMInput kbmInput;
    public void OnFlashlight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    public void OnFreeze(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            kbmInput.enabled = !kbmInput.enabled;
        }
    }
}

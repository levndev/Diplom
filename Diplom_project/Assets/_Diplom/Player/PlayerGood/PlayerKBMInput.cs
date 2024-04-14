using ProceduralToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKBMInput : MonoBehaviour
{
    [SerializeField] private Reference<Vector3> lookDirection;
    [SerializeField] private Reference<Vector3> moveDirection;
    [SerializeField] private Reference<float> moveVertical;

    [SerializeField] private Reference<Vector3> moveSpeedModifier;
    [SerializeField] private float mouseSensitivity;

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        Vector2 delta = context.ReadValue<Vector2>();
        delta = delta * mouseSensitivity;
        lookDirection.Set(new Vector3(lookDirection.Get().x - delta.y, lookDirection.Get().y + delta.x, lookDirection.Get().z));

        lookDirection.Set(new Vector3(Mathf.Clamp(lookDirection.Get().x, -89, 89), lookDirection.Get().y , lookDirection.Get().z));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        var vec = context.ReadValue<Vector2>();
        moveDirection.Set(new Vector3(vec.x, 0, vec.y));
    }

    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        float vertical = context.ReadValue<float>();
        moveVertical.Set(vertical);
    }

    public void OnSlow(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveSpeedModifier.Set(new Vector3(0.5f, 0.5f, 0.5f));
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveSpeedModifier.Set(new Vector3(1, 1, 1));
        }
    }
}

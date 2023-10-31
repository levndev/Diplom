using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKBMInput : MonoBehaviour
{
    [SerializeField] private Reference<Vector3> lookDirection;
    [SerializeField] private Reference<Vector3> moveDirection;
    [SerializeField] private float mouseSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        delta = delta * mouseSensitivity;
        delta = delta * Time.deltaTime;
        lookDirection.Set(new Vector3(lookDirection.Get().x - delta.y, lookDirection.Get().y + delta.x, lookDirection.Get().z));

        lookDirection.Set(new Vector3(Mathf.Clamp(lookDirection.Get().x, -89, 89), lookDirection.Get().y , lookDirection.Get().z));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection.Set(context.ReadValue<Vector3>());
    }
}

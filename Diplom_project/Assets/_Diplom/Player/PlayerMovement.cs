using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Reference<Vector3> moveSpeed;
    [SerializeField] private Reference<float> maxVelocitySqr;
    [SerializeField] private Reference<float> stopDrag;
    [SerializeField] private Reference<float> normalDrag;
    [SerializeField] private Reference<Vector3> moveDirection;
    [SerializeField] private Reference<Vector3> lookDirection;
    [SerializeField] private Rigidbody body;

    private void OnEnable()
    {
        body.drag = normalDrag.Get();
    }

    private void FixedUpdate()
    {
        if (body != null)
        {
            Vector3 move = moveDirection.Get();
            Vector3 speed = moveSpeed.Get();
            if (move != Vector3.zero)
            {
                body.drag = normalDrag.Get();
                if (body.velocity.sqrMagnitude < maxVelocitySqr.Get())
                {
                    body.AddForce(new Vector3(0, move.y * speed.y, 0) * Time.fixedDeltaTime);
                    body.AddForce(transform.rotation * new Vector3(move.x * speed.x, 0, move.z * speed.z) * Time.fixedDeltaTime);
                }
            }
            else
            {
                body.drag = stopDrag.Get();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(lookDirection.Get());
    }
}

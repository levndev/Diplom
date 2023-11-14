using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnappyMovement : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private Reference<Vector3> moveDirection;
    [SerializeField] private Reference<Vector3> lookDirection;
    [SerializeField] private Reference<Vector3> moveSpeedModifier;
    [SerializeField] private Rigidbody rb;

    private void FixedUpdate()
    {
        var rotation = Quaternion.Euler(lookDirection.Get());
        var velocity = rotation * moveDirection.Get();
        velocity.Scale(speed);
        velocity.Scale(moveSpeedModifier.Get());
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
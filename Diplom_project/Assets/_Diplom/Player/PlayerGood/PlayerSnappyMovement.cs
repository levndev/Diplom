using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnappyMovement : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private Reference<Vector3> moveDirection;
    [SerializeField] private Reference<float> moveVertical;
    [SerializeField] private Reference<Vector3> lookDirection;
    [SerializeField] private Reference<Vector3> moveSpeedModifier;
    [SerializeField] private Rigidbody rb;

    private void FixedUpdate()
    {
        var rotation = Quaternion.Euler(lookDirection.Get());
        var dir = moveDirection.Get();;
        var velocity = rotation * dir;
        velocity.Scale(speed);
        velocity.Scale(moveSpeedModifier.Get());
        velocity.y += moveVertical.Get() * speed.y * moveSpeedModifier.Get().y;
        rb.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
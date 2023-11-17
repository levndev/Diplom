using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgentMovement : IAgentMovement
{
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    public override UpdateMode GetUpdateMode()
    {
        return UpdateMode.Update;
    }

    public override void MoveTowards(Vector3Int goal)
    {
        var direction = goal - transform.position;
        var distance = direction.magnitude;
        direction.Normalize();
        var move = speed * Time.deltaTime;
        if (move > distance)
        {
            transform.position += direction * distance;
        }
        else
        {
            transform.position += direction * move;
        }
        float singleStep = Mathf.Deg2Rad * turnSpeed * Time.deltaTime;
        var newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgentMovement : IAgentMovement
{
    public override UpdateMode GetUpdateMode()
    {
        return UpdateMode.Update;
    }

    public override void MoveTowards(Vector3Int goal)
    {
        MoveTowards(goal.ToVector3());
    }

    public override void MoveTowards(Vector3 goal)
    {
        var direction = goal - transform.position;
        var distance = direction.magnitude;
        direction.Normalize();
        var move = moveSpeed * Time.deltaTime;
        if (move > distance)
        {
            transform.position += direction * distance;
        }
        else
        {
            transform.position += direction * move;
        }
        LookTowards(goal);
    }

    public override void LookTowards(Vector3 target)
    {
        float singleStep = Mathf.Deg2Rad * turnSpeed * Time.deltaTime;
        var direction = target - transform.position;
        direction.Normalize();
        var newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}

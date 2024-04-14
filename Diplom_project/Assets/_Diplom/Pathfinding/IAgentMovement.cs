using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAgentMovement : MonoBehaviour
{
    protected float moveSpeed;
    protected float turnSpeed;
    public enum UpdateMode
    {
        FixedUpdate,
        Update
    }

    public abstract void MoveTowards(Vector3Int goal);
    public abstract void MoveTowards(Vector3 goal);
    public abstract void LookTowards(Vector3 target);
    public abstract UpdateMode GetUpdateMode();

    public void setMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    public void setTurnSpeed(float turnSpeed)
    {
        this.turnSpeed = turnSpeed;
    }
}

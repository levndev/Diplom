using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAgentMovement : MonoBehaviour
{
    public enum UpdateMode
    {
        FixedUpdate,
        Update
    }

    public abstract void MoveTowards(Vector3Int goal);
    public abstract void LookTowards(Vector3 target);
    public abstract UpdateMode GetUpdateMode();
}

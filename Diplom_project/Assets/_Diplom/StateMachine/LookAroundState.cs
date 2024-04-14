using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundState : State
{
    public static readonly string StateName = "LookAround";

    Transform rotateTransform;
    Vector3 rotationSpeed;

    Quaternion initialRotation;
    float maxTime;
    float accumulatedTime;
    public LookAroundState(Transform rotateTransform, Vector3 rotationSpeed, float maxTime) : base(StateName)
    {
        this.rotateTransform = rotateTransform;
        this.rotationSpeed = rotationSpeed;
        this.maxTime = maxTime;
    }

    public override void Reset()
    {
        base.Reset();
        accumulatedTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;
        float time = Time.deltaTime;
        if (accumulatedTime + time >= maxTime)
        {
            time = maxTime - accumulatedTime;
            rotateTransform.Rotate(rotationSpeed * time, Space.World);
            requestSwitchState?.Invoke(PatrolState.StateName, false);
            return;
        }
        accumulatedTime += time;
        rotateTransform.Rotate(rotationSpeed * time, Space.World);
    }

    public override void OnActivate()
    {
        base.OnActivate();
        initialRotation = rotateTransform.rotation;
    }

    public override void OnDeactivate()
    {
        rotateTransform.rotation = initialRotation;
        base.OnDeactivate();
    }
}

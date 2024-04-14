using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected bool active;
    protected string name;

    public Action<string, bool> requestSwitchState;

    protected State(string name)
    {
        this.name = name;
    }

    public string Name()
    {
        return name;
    }

    public virtual void Reset()
    {

    }

    public virtual void OnActivate()
    {
        active = true;
    }

    public virtual void OnDeactivate()
    {
        active = false;
    }

    public virtual void Update()
    {
        if (!active)
            return;
    }
    public virtual void FixedUpdate()
    {
        if (!active)
            return;
    }
}

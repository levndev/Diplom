using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public static readonly string StateName = "Chase";
    
    public ChaseState() : base(StateName)
    {

    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnActivate()
    {
        base.OnActivate();
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
    }


    public void OnAgentGoalReached()
    {
        if (!active)
            return;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public static readonly string StateName = "Chase";
    private GameObject target = null;

    private float attackDamage = 1;
    private float attackTimeout = 3;
    private float attackRange = 1;
    private float timeLeftUntilAttack = 0;

    private NavAgent agent;
    private ViewCone viewCone;

    private float moveSpeed = 2;
    private float turnSpeed = 180;

    bool tracking = false;
    private float trackingTime = 1;
    private float trackingTimeLeft = 0;

    public ChaseState(NavAgent agent, ViewCone viewCone) : base(StateName)
    {
        this.agent = agent;
        this.viewCone = viewCone;
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
    }

    public override void Update()
    {
        base.Update();
        timeLeftUntilAttack -= Time.deltaTime;
        if (timeLeftUntilAttack <= 0)
            timeLeftUntilAttack = 0;

        if (tracking)
        {
            trackingTimeLeft -= Time.deltaTime;
            if (trackingTimeLeft <= 0)
                trackingTimeLeft = 0;
        }
        if (target != null)
        {
            Vector3 myPosition = agent.transform.position;
            Vector3 targetPosition = target.transform.position;
            Vector3 diff = targetPosition - myPosition;
            float distance = diff.magnitude;

            if (distance <= attackRange && timeLeftUntilAttack <= 0)
            {
                var health = target.GetComponent<Health>();
                if (health != null)
                {
                    timeLeftUntilAttack = attackTimeout;
                    health.TakeDamage(attackDamage);
                }
            }


            bool hit = Physics.Raycast(myPosition, diff.normalized, distance, LayerMask.GetMask("LevelGeometry"));
            if (hit) // not visible
            {
                if (tracking)
                {
                    if (trackingTimeLeft >= 0)
                    {
                        agent.PathMoveTo(targetPosition.ToVector3Int(), NavAgent.PathMode.Shortening);
                    }
                }
                else
                {
                    tracking = true;
                    trackingTimeLeft = trackingTime;
                    viewCone.setColorMode(2);
                }
            }
            else
            {
                if (!tracking)
                {
                    agent.DirectMoveTo(targetPosition);
                }
            }
        }
    }

    public override void OnActivate()
    {
        base.OnActivate();
        agent.setMoveSpeed(moveSpeed);
        agent.setTurnSpeed(turnSpeed);
        agent.OnGoalReached.AddListener(OnAgentGoalReached);
        viewCone.targetSeen.AddListener(OnViewConeTargetSeen);
        viewCone.setColorMode(1);
    }

    public override void OnDeactivate()
    {
        viewCone.setColorMode(0);
        viewCone.targetSeen.RemoveListener(OnViewConeTargetSeen);
        agent.OnGoalReached.RemoveListener(OnAgentGoalReached);
        base.OnDeactivate();
    }

    public void OnViewConeTargetSeen(GameObject target)
    {
        if (tracking)
        {
            tracking = false;
            viewCone.setColorMode(1);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void OnAgentGoalReached()
    {
        if (!active)
            return;
    }

}

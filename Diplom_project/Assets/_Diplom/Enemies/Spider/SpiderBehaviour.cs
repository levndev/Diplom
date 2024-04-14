using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBehaviour : MonoBehaviour
{
    [SerializeField] private Reference<GameObject> player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private List<GameObject> patrolRoute;
    [SerializeField] private float patrolNodeIdleTime;
    [SerializeField] private State startState;
    [SerializeField] private float patrolNodeApproachDistance;
    private enum State
    {
        Idle,
        PatrollingForward,
        PatrollingBackward,
        MovingToLastSeen,
        InPursuit
    }
    [RuntimeRO]
    private State state;
    private State lastState;
    private int currentPatrolNode = 0;

    private void Awake()
    {
        SetState(startState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Idle)
        {
            animator.SetFloat("Strafe", 0);
            animator.SetFloat("Forward", 0);
        }
        else
        {
            Vector3 direction = agent.velocity;
            direction.Normalize();
            direction = transform.InverseTransformDirection(direction);
            float x = direction.x;
            float z = direction.z;
            animator.SetFloat("Strafe", x);
            animator.SetFloat("Forward", z);
        }
        
        if (state == State.PatrollingForward)
        {
            if (!agent.pathPending && agent.remainingDistance < patrolNodeApproachDistance)
            {
                currentPatrolNode++;
                if (currentPatrolNode >= patrolRoute.Count)
                {
                    currentPatrolNode = patrolRoute.Count - 1;
                    SetState(State.PatrollingBackward);
                }
                GoToNextPatrolNode();
            }
        }
        else if (state == State.PatrollingBackward)
        {
            if (!agent.pathPending && agent.remainingDistance < patrolNodeApproachDistance)
            {
                currentPatrolNode--;
                if (currentPatrolNode < 0)
                {
                    currentPatrolNode = 0;
                    SetState(State.PatrollingForward);
                }
                GoToNextPatrolNode();
            }
        }
        else if (state == State.InPursuit)
        {
            if (player.IsValid())
            {
                agent.destination = player.Get().transform.position;
            }
        }
        else if (state == State.MovingToLastSeen)
        {
            if (!agent.pathPending && agent.remainingDistance < patrolNodeApproachDistance)
            {
                SetState(lastState);
            }
        }
    }

    private void GoToNextPatrolNode()
    {
        if (currentPatrolNode >= 0 && currentPatrolNode < patrolRoute.Count)
        {
            if (patrolRoute[currentPatrolNode] != null)
            {
                agent.destination = patrolRoute[currentPatrolNode].transform.position;
            }
        }
    }


    public void onPlayerSeen()
    {
        if (player.IsValid())
        {
            SetState(State.InPursuit);
            agent.destination = player.Get().transform.position;
        }
    }

    void onPlayerLost()
    {
        if (player.IsValid())
        {
            
        }
    }

    public void onPlayerDetected()
    {
        if (player.IsValid())
        {
            SetState(State.MovingToLastSeen);
            agent.destination = player.Get().transform.position;
        }
    }

    private void SetState(State value)
    {
        if (state == value)
            return;
        lastState = state;
        state = value;
        switch (state)
        {
            case State.Idle:
                break;
            case State.PatrollingForward:
            case State.PatrollingBackward:
                GoToNextPatrolNode();
                break;
            case State.MovingToLastSeen:
                break;
            case State.InPursuit:
                break;
            default:
                break;
        }
    }
}

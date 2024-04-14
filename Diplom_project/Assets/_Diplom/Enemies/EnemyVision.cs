using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private Reference<float> playerLightIntensity;
    [SerializeField] private Reference<float> playerLightIntensityThreshold;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onViewConeTargetSeen(GameObject target)
    {
        if (playerLightIntensity.Get() >= playerLightIntensityThreshold.Get())
        {
            stateMachine.SwitchToState(ChaseState.StateName, true);
            var state = stateMachine.CurrentState() as ChaseState;
            state.SetTarget(target);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIIdleState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new(nameof(AIPatrolState));
        transition.AddCondition(new FloatCondition(agent.timer, Condition.Predicate.LESS, 0));
        transitions.Add(transition);

        transition = new(nameof(AIChaseState));
        transition.AddCondition(new BoolCondition(agent.enemySeen));
        transitions.Add(transition);

    }

    public override void OnEnter()
    {
        agent.movement.Stop();
        agent.movement.Velocity = Vector3.zero;

        agent.timer.value = Random.Range(1, 2);
    }

    public override void OnExit()
    {
        Debug.Log("Idle Exit");
    }

    public override void OnUpdate()
    {

    }
}

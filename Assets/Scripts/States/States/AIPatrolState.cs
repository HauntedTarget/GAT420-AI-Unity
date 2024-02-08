using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolState : AIState
{
    public AIPatrolState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new(nameof(AIIdleState));
        transition.AddCondition(new FloatCondition(agent.destinationDistance, Condition.Predicate.LESS, 1));
        transitions.Add(transition);

        transition = new(nameof(AIChaseState));
        transition.AddCondition(new BoolCondition(agent.enemySeen));
        transitions.Add(transition);


    }

    public override void OnEnter()
    {
        agent.movement.Resume();

        agent.movement.Destination = AINavNode.GetRandomAINavNode().transform.position;
    }

    public override void OnUpdate()
    {
        // Move to random node in patrol
        agent.movement.MoveTowards(agent.movement.Destination);
    }

    public override void OnExit()
    {
        Debug.Log("Patrol Exit");
    }
}

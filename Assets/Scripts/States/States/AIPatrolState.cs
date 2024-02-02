using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolState : AIState
{
    Vector3 destination;

    public AIPatrolState(AIStateAgent agent) : base(agent)
    {

    }

    public override void OnEnter()
    {
        destination = AINavNode.GetRandomAINavNode().transform.position;
    }

    public override void OnUpdate()
    {
        // Move to random node in patrol
        agent.movement.MoveTowards(destination);

        // On arival, Idle
        if (Vector3.Distance(destination, agent.transform.position) < 1) agent.stateMachine.SetState(nameof(AIIdleState));

        var enemies = agent.enemyPerception.GetGameobjects();
        if (enemies.Length > 0) agent.stateMachine.SetState(nameof(AIChaseState));
    }

    public override void OnExit()
    {

    }
}

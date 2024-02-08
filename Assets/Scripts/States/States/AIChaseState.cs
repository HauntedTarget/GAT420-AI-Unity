using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIState
{
    float initialSpeed;

    public AIChaseState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new(nameof(AIAttackState));
        transition.AddCondition(new FloatCondition(agent.enemyDistance, Condition.Predicate.LESS, 2));
        transition.AddCondition(new BoolCondition(agent.enemySeen));
        transitions.Add(transition);

        transition = new(nameof(AIIdleState));
        transition.AddCondition(new FloatCondition(agent.enemyDistance, Condition.Predicate.GREATER, 5));
        transition.AddCondition(new BoolCondition(agent.enemySeen, false));
        transitions.Add(transition);
    }

    public override void OnEnter()
    {
        agent.movement.Resume();
        initialSpeed = agent.movement.maxSpeed;
        agent.movement.maxSpeed *= 2;
    }

    public override void OnUpdate()
    {
        if (agent.enemySeen && agent.enemy != null) agent.movement.Destination = agent.enemy.transform.position;
    }

    public override void OnExit()
    {
        agent.movement.maxSpeed = initialSpeed;
    }
}

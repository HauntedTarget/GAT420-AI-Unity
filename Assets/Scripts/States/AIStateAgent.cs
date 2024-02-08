using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAgent : AIAgent
{
    public Animator animator;
    public AIPerception enemyPerception;

    //Parameters
    public ValueRef<float> health = new(),  // -> Memory
        timer = new(),                      // -> Memory
        destinationDistance = new(),        // -> Memory
        enemyDistance = new(),              // -> Memory
        enemyHealth = new();                // -> Memory
    public ValueRef<bool> enemySeen = new();// -> Memory

    public AIStateMachine stateMachine = new();
    public AIStateAgent enemy { get; private set; }

    void Start()
    {
        health.value = 100;

        //Add states to state machine
        stateMachine.AddState(nameof(AIIdleState), new AIIdleState(this));
        stateMachine.AddState(nameof(AIDeathState), new AIDeathState(this));
        stateMachine.AddState(nameof(AIPatrolState), new AIPatrolState(this));
        stateMachine.AddState(nameof(AIAttackState), new AIAttackState(this));
        stateMachine.AddState(nameof(AIChaseState), new AIChaseState(this));

        stateMachine.SetState(nameof(AIIdleState));
    }

    private void Update()
    {
        // Update Parameters
        timer.value -= Time.deltaTime;
        destinationDistance.value = Vector3.Distance(transform.position, movement.Destination);
        var enemies = enemyPerception.GetGameobjects();
        enemySeen.value = enemies.Length > 0 ? true : false;
        if (enemySeen)
        {
            enemy = enemies[0].TryGetComponent(out AIStateAgent stateAgent) ? stateAgent : null;
            enemyDistance.value = Vector3.Distance(transform.position, enemy.transform.position);
            enemyHealth.value = enemy.health;
        }

        // From any state (health -> death)
        if (health <= 0) stateMachine.SetState(nameof(AIDeathState));

        animator?.SetFloat("Speed", movement.Velocity.magnitude);

        // Check for transition
        foreach (var transition in stateMachine.CurrentState.transitions)
        {
            if (transition.ToTransition())
            {
                stateMachine.SetState(transition.nextState);
                break;
            }
        }
    }

    private void OnGUI()
    {
        // draw label of current state above agent
        GUI.backgroundColor = Color.black;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(0, 0, 100, 20);
        // get point above agent
        Vector3 point;
        if (health > 0)
        {
            point = Camera.main.WorldToScreenPoint(transform.position);
        }
        else
        {
            point = Camera.main.transform.position;
        }
        rect.x = point.x - (rect.width / 2);
        rect.y = Screen.height - point.y - rect.height - 20;
        // draw label with current state name
        GUI.Label(rect, stateMachine.CurrentState.name);
    }

    public void ApplyDamage(float damage)
    {
        health.value -= damage;
        if (health > 0) stateMachine.SetState(nameof(AIDeathState));
    }

    public void Attack()
    {
        // check for collision with surroundings
        var colliders = Physics.OverlapSphere(transform.position, 1);
        foreach (var collider in colliders)
        {
            // don't hit self or objects with the same tag
            if (collider.gameObject == gameObject || collider.gameObject.CompareTag(gameObject.tag)) continue;

            // check if collider object is a state agent, reduce health
            if (collider.gameObject.TryGetComponent<AIStateAgent>(out var stateAgent))
            {
                stateAgent.ApplyDamage(UnityEngine.Random.Range(20, 50));
            }
        }
    }
}

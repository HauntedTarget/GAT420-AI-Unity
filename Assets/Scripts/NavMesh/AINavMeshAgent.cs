using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavMeshAgent : MonoBehaviour
{
    public NavMeshAgent agent;

    public AINavNode navDestination;

    private void Start()
    {
        agent.SetDestination(navDestination.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desTest = new(navDestination.transform.position.x, transform.position.y, navDestination.transform.position.z);
        if (transform.position == desTest && navDestination.neighbors.Count > 0)
        {
            agent.SetDestination(navDestination.neighbors[0].transform.position);
            navDestination = navDestination.neighbors[0];
        }
    }
}

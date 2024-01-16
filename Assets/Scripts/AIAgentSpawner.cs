using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgentSpawner : MonoBehaviour
{
    [SerializeField] AIAgent[] agents;
    [SerializeField] LayerMask layerMask;

    int index = 0;

    void Update()
    {
        // switch agent to spawn
        if (Input.GetKeyDown(KeyCode.Tab)) index = (++index % agents.Length);

        // click to spawn; hold lControl to spawn multiple
        if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)))
        {
            // get ray from camera to screen position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // raycast to see if it hits on a layer
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
            {
                // spawn at point (Random rotation)
                Instantiate(agents[index], hitInfo.point, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public GameObject sentinel;

    // TODO: these are temporary values
    const int droneInterval = 2; // minimum interval spawned drones must have between each other
    const int lowLimit = -2; // the lowest y for spawned drones
    const int topLimit = 10; // the highest y for spawned drones
    const float droneProb = 0.4f; // chance of spawning drone at each step

    float side = 1f; // which side of the brifge this tower is in (1 for positive x; -1 for negative x)

    // Start is called before the first frame update
    void Start()
    {
        side = Mathf.Sign(transform.position.x);

        if (sentinel) SpawnDrones();
        else Debug.Log("Drone Prefab not defined");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: optimise
    // TODO: spawn different drones
    void SpawnDrones() {
        for (int i = lowLimit; i < topLimit; i++) {
            if (Random.Range(0f, 1f) <= droneProb) {
                GameObject newDrone = Instantiate(sentinel);

                float y = i;

                // near side face
                float x = transform.position.x;
                float z = transform.position.z - 1.5f;

                float face = Random.Range(0f, 1f);
                if (face < 0.35) { // bridge side face
                    x = transform.position.x - side * 1.5f;
                    z = transform.position.z;
                }
                else if (face < 0.5) { // chasm side face
                    x = transform.position.x - side * 1.5f;
                    z = transform.position.z;
                }
                else if (face < 0.65) { // far side face
                    x = transform.position.x;
                    z = transform.position.z + 1.5f;
                }

                newDrone.transform.position = new Vector3(x,y,z);

                i += droneInterval;
            }
        }
    }
}

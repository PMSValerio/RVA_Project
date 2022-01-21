using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private GameObject sentinel;

    // TODO: these are temporary values
    private const int droneInterval = 2; // minimum interval spawned drones must have between each other
    private const int lowLimit = -2; // the lowest y for spawned drones
    private const int topLimit = 10; // the highest y for spawned drones
    private float droneProb; // chance of spawning drone at each step

    float side = 1f; // which side of the bridge this tower is in (1 for positive x; -1 for negative x)

    // Start is called before the first frame update
    void Start() {
        //side = Mathf.Sign(transform.position.x);
        droneProb = GameManager.Instance.GetDroneSpawnProbability();
        
        if (sentinel) SpawnDrones();
        else Debug.Log("Drone Prefab not defined");
    }

    // TODO: optimise
    // TODO: spawn different drones
    void SpawnDrones() {
        for (int i = lowLimit; i < topLimit; i++) {
            if (Random.Range(0f, 1f) <= droneProb) {
                float y = i;
                
                // near side face
                float x = 0;
                float z = -1.5f;

                float face = Random.Range(0f, 1f);
                if (face < 0.35) { // bridge side face
                    x = -side * 1.5f;
                    z = 0;
                }
                else if (face < 0.5) { // chasm side face
                    x = -side * 1.5f;
                    z = 0;
                }
                else if (face < 0.65) { // far side face
                    x = 0;
                    z = 1.5f;
                }
                
                Vector3 forward = new Vector3(x,y,z);
                GameObject newDrone = Instantiate(sentinel);
                newDrone.transform.position = transform.position + forward;
                forward = new Vector3(forward.x,0,forward.z);
                newDrone.transform.localRotation = Quaternion.LookRotation(forward,Vector3.up);
                
                GameManager.Instance.IncrementNumEnemies();

                i += droneInterval;
            }
        }
    }
    
}

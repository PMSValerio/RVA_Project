using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private GameObject sentinel;

    // TODO: these are temporary values
    private const int droneInterval = 2; // minimum interval spawned drones must have between each other
    private const int lowLimit = -2; // the lowest y for spawned drones
    private const int topLimit = 10; // the highest y for spawned drones
    private const float droneProb = 0.2f; // chance of spawning drone at each step

    float side = 1f; // which side of the bridge this tower is in (1 for positive x; -1 for negative x)

    // Start is called before the first frame update
    void Start() {
        //side = Mathf.Sign(transform.position.x);

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
                float x = transform.position.x;
                float z = transform.position.z - 1.5f;

                float face = Random.Range(0f, 1f);
                if (face < 0.35) { // bridge side face
                    Debug.Log("A");
                    x = transform.position.x - side * 1.5f;
                    z = transform.position.z;
                    Instantiate(sentinel, new Vector3(x,y,z), Quaternion.LookRotation(Vector3.back));
                }
                else if (face < 0.5) { // chasm side face
                    Debug.Log("B");
                    x = transform.position.x + side * 1.5f;
                    z = transform.position.z;
                    Instantiate(sentinel, new Vector3(x,y,z), Quaternion.LookRotation(Vector3.forward));
                }
                else if (face < 0.65) { // far side face
                    Debug.Log("C");
                    x = transform.position.x;
                    z = transform.position.z + 1.5f;
                    Instantiate(sentinel, new Vector3(x,y,z), Quaternion.LookRotation(Vector3.left));
                }
                else {
                    Debug.Log("D");
                    Instantiate(sentinel, new Vector3(x,y,z), Quaternion.LookRotation(Vector3.right));
                }

                
                Debug.Log(new Vector3(x,y,z));

                i += droneInterval;
            }
        }
    }
}

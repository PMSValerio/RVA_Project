using UnityEngine;
using UnityEngine.AI;

public class PathGenerator : MonoBehaviour {

    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject goal;
    private int numBridges;

    private const float goalHeight = 3f;
    
    private void Awake() {
        numBridges = GameManager.Instance.GetNumBridges();
        GeneratePath();
    }

    private void GeneratePath() {
        Transform previousBridge = GameObject.Find("Bridge").transform;

        int previousDirection = 1;

        for (int i = 0; i < numBridges; i++) {
            Vector3 previousScale = previousBridge.localScale;
            Vector3 newScale = new Vector3(previousScale.x, previousScale.y, Random.Range(1f,5f));

            Vector3 nextPosition = default;

            int newDirection = GetNewDirection(previousDirection);

            switch (previousDirection) {
                // Left path
                case 0:
                    nextPosition = newDirection == 1 ? new Vector3(previousBridge.position.x - 10*previousScale.z/2 - 10*newScale.x/2, previousBridge.position.y, previousBridge.position.z + 10*newScale.z/2 - 10*previousScale.x/2) : new Vector3(previousBridge.position.x - 10*previousScale.z/2 - 10*newScale.z/2, previousBridge.position.y, previousBridge.position.z);
                    break;
                // Front path
                case 1:
                    nextPosition = newDirection switch {
                        0 => new Vector3(previousBridge.position.x - 10 * newScale.z / 2 + 10 * previousScale.x / 2,
                            previousBridge.position.y,
                            previousBridge.position.z + 10 * previousScale.z / 2 + 10 * previousScale.x / 2),
                        1 => new Vector3(previousBridge.position.x, previousBridge.position.y,
                            previousBridge.position.z + 10 * previousScale.z / 2 + 10 * newScale.z / 2),
                        2 => new Vector3(previousBridge.position.x + 10 * newScale.z / 2 - 10 * previousScale.x / 2,
                            previousBridge.position.y,
                            previousBridge.position.z + 10 * previousScale.z / 2 + 10 * previousScale.x / 2),
                        _ => nextPosition
                    };
                    break;
                // Right path
                case 2:
                    nextPosition = newDirection == 1 ? new Vector3(previousBridge.position.x + 10*previousScale.z/2 + 10*newScale.x/2, previousBridge.position.y, previousBridge.position.z + 10*newScale.z/2 - 10*previousScale.x/2) : new Vector3(previousBridge.position.x + 10*previousScale.z/2 + 10*newScale.z/2, previousBridge.position.y, previousBridge.position.z);
                    break;
            }
            
            previousBridge = Instantiate(bridge, nextPosition, Quaternion.identity, transform).transform;
            previousBridge.transform.localScale = new Vector3(newScale.x, newScale.y, newScale.z);
            // Rotate if newBridge will be sideways
            if (newDirection != 1) previousBridge.Rotate(Vector3.up, 90);
            
            previousDirection = newDirection;
        }
        
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        
        // Spawn Goal
        SpawnGoal(previousDirection, previousBridge);
    }

    private void SpawnGoal(int direction, Transform bridge) {
        Vector3 target = default;
        switch (direction) {
            // Left path
            case 0:
                target = new Vector3(bridge.transform.position.x - 10 * bridge.localScale.z/2, bridge.transform.position.y + goalHeight, bridge.transform.position.z);
                break;
            // Front path
            case 1:
                target = new Vector3(bridge.transform.position.x, bridge.transform.position.y + goalHeight, bridge.transform.position.z + 10*bridge.localScale.z/2);
                break;
            // Right path
            case 2:
                target = new Vector3(bridge.transform.position.x + 10*bridge.localScale.z/2, bridge.transform.position.y + goalHeight, bridge.transform.position.z);
                break;
        }
        
        Instantiate(goal, target, Quaternion.identity, transform).name = goal.name;
    }
    
    private int GetNewDirection(int previousDirection) {
        int newDirection = Random.Range(0, 3);
        
        // If last direction was front, all directions are available
        if (previousDirection == -1 || previousDirection == 1) return newDirection;
        
        // If last direction was a turn, it can't be the opposite side now
        if ((previousDirection == 0 && newDirection == 2) || (previousDirection == 2 && newDirection == 0)) {
            // Just turn front in this case
            return 1;
        }

        return newDirection;
    }
}

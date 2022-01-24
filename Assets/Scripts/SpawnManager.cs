using System;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    // Spawner attributes
    private const float tolerance = 1; // tolerance between NavMeshPath corners
    private float towerSpawnProbability; // chance of spawning tower at each step
    private const int towerFreeSpace = 2; // minimum distance towers must have between each other and others objects such as the path
    private const int mapDim = 20; // maximum distance towers can be from path
    private const int cornersOffset = 8; // offset for towers to spawn
    
    // Prefab object
    [SerializeField] private GameObject tower;
    
    // Start is called before the first frame update
    private void Start() {
        towerSpawnProbability = GameManager.Instance.GetTowerSpawnProbability();
        
        if (tower) {
            Invoke(nameof(SpawnTowers), 3f);
        }
        else Debug.Log("Tower Prefab not defined");
    }

    private void SpawnTowers() {
        Debug.Log("Spawning towers");
        Vector3 previousCheckpoint = GameManager.Instance.GetPathCheckpoints()[0];

        int numBridges = GameManager.Instance.GetPathCheckpoints().Length - 1;
        Vector3 goal = GameObject.Find("Goal").transform.position;

        foreach (Vector3 actualCheckpoint in GameManager.Instance.GetPathCheckpoints().Skip(1)) {
            numBridges -= 1;
            // Vertical path
            if (Math.Abs(previousCheckpoint.x - actualCheckpoint.x) < tolerance) {
                for (float zz = previousCheckpoint.z-cornersOffset; zz < actualCheckpoint.z+cornersOffset; zz++) {
                    for (float xx = previousCheckpoint.x-mapDim; xx < previousCheckpoint.x+mapDim; xx++) {
                        /*Debug.Log(numBridges);
                        Debug.Log("Goal " + goal.x + ", " + goal.z);
                        Debug.Log("Tower " + xx + ", " + zz);*/
                        if (numBridges == 0 && Mathf.Abs(Mathf.Abs(goal.x)-Mathf.Abs(xx)) < 2 && goal.z >= zz) {
                            continue;
                        }
                        //Debug.Log("Spawning");
                        if (UnityEngine.Random.Range(0f,1f) < towerSpawnProbability) {
                            Vector3 spawnPosition = new Vector3(xx, 0, zz);
                            
                            // Check if we can spawn a tower there (aka no collision with other objects such as path and other towers)
                            Collider[] hitColliders = new Collider[1];
                            int numHits = Physics.OverlapSphereNonAlloc(spawnPosition, towerFreeSpace, hitColliders);
                            if (numHits < 1) {
                                Instantiate(tower, spawnPosition, Quaternion.identity, transform);
                                xx += towerFreeSpace;
                                // Necessary so that OverlapSphere takes into account instantiated objects in the same frame
                                Physics.SyncTransforms();
                            }
                        }
                    }
                }
            }
            // Horizontal path to the right
            else if (Math.Abs(previousCheckpoint.z - actualCheckpoint.z) < tolerance) {
                if (previousCheckpoint.x < actualCheckpoint.x) {
                    for (float xx = previousCheckpoint.x-cornersOffset; xx < actualCheckpoint.x+cornersOffset; xx++) {
                        for (float zz = previousCheckpoint.z-mapDim; zz < previousCheckpoint.z+mapDim; zz++) {
                            /*Debug.Log(numBridges);
                            Debug.Log("Goal " + goal.x + ", " + goal.z);
                            Debug.Log("Tower " + xx + ", " + zz);*/
                            if (numBridges == 0 && Mathf.Abs(Mathf.Abs(goal.z)-Mathf.Abs(zz)) < 2 && goal.x >= xx) {
                                continue;
                            }
                            //Debug.Log("Spawning");
                            if (UnityEngine.Random.Range(0f,1f) < towerSpawnProbability) {
                                Vector3 spawnPosition = new Vector3(xx, 0, zz);
                            
                                // Check if we can spawn a tower there (aka no collision with other objects such as path and other towers)
                                Collider[] hitColliders = new Collider[1];
                                int numHits = Physics.OverlapSphereNonAlloc(spawnPosition, towerFreeSpace, hitColliders);
                                if (numHits < 1) {
                                    Instantiate(tower, spawnPosition, Quaternion.identity, transform);
                                    zz += towerFreeSpace;
                                    // Necessary so that OverlapSphere takes into account instantiated objects in the same frame
                                    Physics.SyncTransforms();
                                }
                            }
                        }
                    }
                }
                // Horizontal path to the left
                else {
                    for (float xx = previousCheckpoint.x+cornersOffset; xx > actualCheckpoint.x-cornersOffset; xx--) {
                        for (float zz = previousCheckpoint.z-mapDim; zz < previousCheckpoint.z+mapDim; zz++) {
                            /*Debug.Log(numBridges);
                            Debug.Log("Goal " + goal.x + ", " + goal.z);
                            Debug.Log("Tower " + xx + ", " + zz);*/
                            if (numBridges == 0 && Mathf.Abs(Mathf.Abs(goal.z)-Mathf.Abs(zz)) < 2 && goal.x <= xx) {
                                continue;
                            }
                            //Debug.Log("Spawning");
                            if (UnityEngine.Random.Range(0f,1f) < towerSpawnProbability) {
                                Vector3 spawnPosition = new Vector3(xx, 0, zz);
                            
                                // Check if we can spawn a tower there (aka no collision with other objects such as path and other towers)
                                Collider[] hitColliders = new Collider[1];
                                int numHits = Physics.OverlapSphereNonAlloc(spawnPosition, towerFreeSpace, hitColliders);
                                if (numHits < 1) {
                                    Instantiate(tower, spawnPosition, Quaternion.identity, transform);
                                    zz += towerFreeSpace;
                                    // Necessary so that OverlapSphere takes into account instantiated objects in the same frame
                                    Physics.SyncTransforms();
                                }
                            }
                        }
                    }
                }
                
            }
            // Else are corners
            previousCheckpoint = actualCheckpoint;
        }
    }
}

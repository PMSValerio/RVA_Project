using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{

    public GameObject tower;

    Vector2 mapDim = new Vector2(20,40); // map half witdh and depth

    Vector2 towerFree = new Vector2(2,4); // minimum distance, sideways and forwards respectively, towers must have between each other
    float bridgeOffset = 7; // distance between bridge and tower spawning area
    float edgeOffset = 3; // distance between each bridge end and tower spawning area

    float towerProb = 0.05f; // chance of spawning tower at each step
    int towerlim = -1;
    int towercount;

    // Start is called before the first frame update
    void Start()
    {
        if (tower) {
            SpawnTowers();
        }
        else Debug.Log("Tower Prefab not defined");
        towercount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTowers() {
        if (towerlim>=0 && towercount>=towerlim) return;
        for (float xx = bridgeOffset; xx < mapDim.x; xx += towerFree.x) {
            // right side
            for (float zz = edgeOffset; zz < mapDim.y; zz += 1) {
                if (Random.Range(0f,1f) < towerProb) {
                    GameObject towerObj = Instantiate(tower);
                    towerObj.transform.position = new Vector3(xx,0,zz);
                    zz += towerFree.y;
                    towercount++;
                    if (towerlim>=0 && towercount>=towerlim) return;
                }
            }
            // left side
            for (float zz = edgeOffset; zz < mapDim.y; zz += 1) {
                if (Random.Range(0f,1f) < towerProb) {
                    GameObject towerObj = Instantiate(tower);
                    towerObj.transform.position = new Vector3(-xx,0,zz);
                    zz += towerFree.y;
                    towercount++;
                    if (towerlim>=0 && towercount>=towerlim) return;
                }
            }
        }
    }
}

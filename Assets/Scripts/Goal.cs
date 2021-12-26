using UnityEngine;

public class Goal : MonoBehaviour {
    
    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnDestroy() {
        GameManager.Instance.ResumeNavMeshAgent();
        GameObject.Find("Platform").GetComponent<Platform>().SetDestinationToGoal(false);
    }
}

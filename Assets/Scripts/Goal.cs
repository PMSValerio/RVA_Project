using UnityEngine;

public class Goal : MonoBehaviour {
    
    // Start is called before the first frame update
    private void Start() {
        GameManager.Instance.IncrementNumEnemies();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnDestroy() {
        Debug.Log("AAA");
        GameManager.Instance.ResumeNavMeshAgent();
        GameObject.Find("Platform").GetComponent<Platform>().SetDestinationToGoal(false);
        GameManager.Instance.DecrementNumEnemies();
        GameManager.Instance.TensionUp();
        if (GameManager.Instance.GetNumEnemies() == 0) {
            GameManager.Instance.Overlay.ToggleOnLevelCompleted();
        }
    }
}

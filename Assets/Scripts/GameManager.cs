using UnityEngine;

public class GameManager : MonoBehaviour {
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // Public variables to access through other scripts
    public GameObject player { get; private set; }

    private bool forward;
    private Vector3[] pathCheckpoints;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
        player = GameObject.Find("Player");
    }
    
    public void SetForward(bool value) {
        forward = value;
    }

    public bool GetForward() {
        return forward;
    }
    
    public void SetPathCheckpoints(Vector3[] value) {
        pathCheckpoints = value;
    }

    public Vector3[] GetPathCheckpoints() {
        return pathCheckpoints;
    }

    
}

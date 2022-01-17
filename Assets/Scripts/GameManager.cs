using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    // Public variables to access through other scripts
    public GameObject Player { get; private set; } // Player GameObject Singleton
    public NavMeshAgent NavMeshAgent { get; private set; } // NavMeshAgent Singleton
    public UIOverlay Overlay { get; private set; } // UI Overlay Singleton

    private bool isOnFirstStage;
    private Vector3[] pathCheckpoints;

    private int numEnemies;
    
    private const float runningNavMeshAgentSpeed = 2.0f; // NavMeshAgent speed while moving

    private bool isGamePaused = true;
    private bool wasAgentRunning = false;
    private bool hasGameStarted = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        Player = GameObject.Find("Player");
        NavMeshAgent = GameObject.Find("Platform").GetComponent<NavMeshAgent>();
        Overlay = GameObject.Find("HUD").GetComponent<UIOverlay>();
        
        NavMeshAgent.updateRotation = false;
        SetIsGamePaused(true);
    }

    public void SetIsOnFirstStage(bool value) {
        isOnFirstStage = value;
    }

    public bool GetIsOnFirstStage() {
        return isOnFirstStage;
    }

    public void SetIsGamePaused(bool value) {
        isGamePaused = value;
        if (value) {
            if (NavMeshAgent.speed > 0.0f) {
                wasAgentRunning = true;
            } else {
                wasAgentRunning = false;
            }
            StopNavMeshAgent();
        } else if (wasAgentRunning) {
            ResumeNavMeshAgent();
        }
    }
    
    public bool GetIsGamePaused() {
        return isGamePaused;
    }
    
    public void SetPathCheckpoints(Vector3[] value) {
        pathCheckpoints = value;
    }

    public Vector3[] GetPathCheckpoints() {
        return pathCheckpoints;
    }

    public void IncrementNumEnemies() {
        numEnemies++;
        Overlay.SetEnemiesAlive(numEnemies);
    }
    
    public void DecrementNumEnemies() {
        numEnemies--;
        Overlay.SetEnemiesAlive(numEnemies);
    }

    public int GetNumEnemies() {
        return numEnemies;
    }

    public bool GetHasGameStarted() {
        return hasGameStarted;
    }
    
    //
    // NavMeshAgent Speed
    //

    // Resume NavMeshAgent
    public void ResumeNavMeshAgent() {
        hasGameStarted = true;
        Overlay.ToggleOnEnemiesAlive();
        NavMeshAgent.speed = runningNavMeshAgentSpeed;
    }
    
    // Stop NavMeshAgent
    public void StopNavMeshAgent() {
        //Overlay.ToggleOffEnemiesAlive();
        NavMeshAgent.speed = 0.0f;
    }
}

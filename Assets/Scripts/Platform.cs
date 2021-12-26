using UnityEngine;
using NavMeshBuilder = UnityEditor.AI.NavMeshBuilder;

public class Platform : MonoBehaviour {

    private Vector3 sourcePosition; // Platform's source position
    private Vector3 destinationPosition; // Platform's destination position
    
    private void Awake() {
        sourcePosition = transform.position;
    }

    private void Start() {
        destinationPosition = GameObject.Find("Goal").transform.position;
        
        SetDestinationToGoal(true);
    }

    private void Update() {
        // Keep the player in the middle of the platform
        GameManager.Instance.Player.transform.position = new Vector3(transform.position.x, GameManager.Instance.Player.transform.position.y, transform.position.z);

        if (GameManager.Instance.NavMeshAgent.path.corners.Length == 1) {
            GameManager.Instance.StopNavMeshAgent();    
        }
        
        if (GameManager.Instance.GetPathCheckpoints() is null || GameManager.Instance.NavMeshAgent.path.corners.Length > GameManager.Instance.GetPathCheckpoints().Length) {
            GameManager.Instance.SetPathCheckpoints(GameManager.Instance.NavMeshAgent.path.corners);
        }
    }

    public void SetDestinationToGoal(bool toGoal) {
        if (toGoal) {
            GameManager.Instance.SetIsOnFirstStage(true);
            GameManager.Instance.NavMeshAgent.SetDestination(destinationPosition);
            return;
        }
        GameManager.Instance.SetIsOnFirstStage(false);
        GameManager.Instance.NavMeshAgent.SetDestination(sourcePosition);
    }
}

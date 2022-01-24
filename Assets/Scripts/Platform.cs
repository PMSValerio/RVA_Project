using UnityEngine;

public class Platform : MonoBehaviour {

    private Vector3 sourcePosition; // Platform's source position
    private Vector3 destinationPosition; // Platform's destination position

    private bool bossFight = false;
    
    private void Awake() {
        sourcePosition = transform.position;
    }

    private void Update() {
        if (GameManager.Instance.NavMeshAgent.path.corners.Length == 1) {
            GameManager.Instance.StopNavMeshAgent();    
        }
        
        /*
        if (GameManager.Instance.GetPathCheckpoints() is null || GameManager.Instance.NavMeshAgent.path.corners.Length > GameManager.Instance.GetPathCheckpoints().Length) {
            GameManager.Instance.SetPathCheckpoints(GameManager.Instance.NavMeshAgent.path.corners);
        }*/
        
        if (GameManager.Instance.GetIsGamePaused()) {
            return;
        }

        if (!bossFight && GameManager.Instance.GetIsOnFirstStage()) {
            if (!GameManager.Instance.NavMeshAgent.pathPending) {
                if (GameManager.Instance.NavMeshAgent.remainingDistance <= GameManager.Instance.NavMeshAgent.stoppingDistance) {
                    if (!GameManager.Instance.NavMeshAgent.hasPath || GameManager.Instance.NavMeshAgent.velocity.sqrMagnitude == 0f) {
                        GameManager.Instance.Overlay.ToggleBossTimer(true);
                        bossFight = true;
                    }
                }
            }
        }
        
        // Keep the player in the middle of the platform
        GameManager.Instance.Player.transform.position = new Vector3(transform.position.x, GameManager.Instance.Player.transform.position.y, transform.position.z);
    }

    public void SetDestinationToGoal(bool toGoal) {
        GameManager.Instance.SetIsOnFirstStage(false);
        GameManager.Instance.NavMeshAgent.SetDestination(sourcePosition);
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Platform : MonoBehaviour {

    private Vector3 sourcePosition; // Platform's source position
    private Vector3 goalPosition; // Platform's destination position
    private NavMeshAgent agent; // NavMesh Agent - (AI movement)

    [SerializeField] private Button startButton;
    
    private void Awake() {
        sourcePosition = transform.position;
        goalPosition = GameObject.Find("Goal").transform.position;
    }

    private void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalPosition);
        agent.updateRotation = false;
        agent.speed = 0;
        GameManager.Instance.SetForward(true);
    }

    private void Update() {
        // Change platform's orientation according to its position
        if (transform.position.x == goalPosition.x && transform.position.z == goalPosition.z && GameManager.Instance.GetForward()) {
            agent.SetDestination(sourcePosition);
            GameManager.Instance.SetForward(false);
        } else if (transform.position.x == sourcePosition.x && transform.position.z == sourcePosition.z && !GameManager.Instance.GetForward()) {
            Debug.Log("CHEGOU");
        }
        
        // Keep the player in the middle of the platform
        GameManager.Instance.Player.transform.position = new Vector3(transform.position.x, GameManager.Instance.Player.transform.position.y, transform.position.z);

        if (agent.path.corners.Length > 1 && GameManager.Instance.GetPathCheckpoints() is null) {
            GameManager.Instance.SetPathCheckpoints(agent.path.corners);
        }
    }

    public void SetAgentSpeed(float value) {
        agent.speed = value;
        startButton.gameObject.SetActive(false);
    }

    public float GetAgentSpeed() {
        return agent.speed;
    }
}

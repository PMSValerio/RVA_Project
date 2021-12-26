using UnityEngine;

public class Enemy : MonoBehaviour {
    // Enemy attributes
    private int hp = 1; // Enemy's health points
    private const float speed = 2; // Enemy's speed

    private bool isDead;

    protected virtual void Start() {
        
    }

    private void Update() {
        // Enemy should only chase the player if the player isn't moving forward
        if (!GameManager.Instance.GetIsOnFirstStage()) {
            transform.LookAt(GameManager.Instance.Player.transform);
            transform.rotation *= Quaternion.Euler(0, -90, 0);
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    // Enemy is hit
    public void Damage(int damage) {
        // Don't do any damage if the game is "paused"
        if ((GameManager.Instance.NavMeshAgent.speed == 0.0f && GameManager.Instance.GetIsOnFirstStage()) || isDead) return;
        
        // Decrease health points
        hp -= damage;
        // If health points are less than or equal to 0, enemy should die
        if (hp <= 0) {
            hp = 0;
            Die();
        }
    }

    // Destroy itself
    private void Die() {
        // Prevent double method call (e.g: enemy's body and eye both hit)
        isDead = true;
        
        GameManager.Instance.DecrementNumEnemies();
        
        // If the player is already on the second stage and there are no more enemies alive, then that level is completed
        if (!GameManager.Instance.GetIsOnFirstStage() && GameManager.Instance.GetNumEnemies() == 0) {
            GameManager.Instance.Overlay.ToggleOnLevelCompleted();
        }
        Destroy(gameObject);
    }
}

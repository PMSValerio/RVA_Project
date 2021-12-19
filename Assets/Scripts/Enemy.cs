using UnityEngine;

public class Enemy : MonoBehaviour {
    // Enemy attributes
    private int hp = 1; // Enemy's health points
    private const float speed = 2; // Enemy's speed

    protected virtual void Start() {
        
    }

    private void Update() {
        // Enemy should only chase the player if the player isn't moving forward
        if (!GameManager.Instance.GetForward()) {
            transform.LookAt(GameManager.Instance.Player.transform);
            transform.rotation *= Quaternion.Euler(0, -90, 0);
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    // Enemy is hit
    public void Damage(int damage) {
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
        Destroy(gameObject);
    }
}

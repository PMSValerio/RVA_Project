using System.Security.Cryptography;
using UnityEngine;

public class PistolBullet : MonoBehaviour {

    public float speed = 32f;
    public int power = 5;

    private const float lifeDuration = 2f;
    private float lifeTimer;

    // Start is called before the first frame update
    void Start() {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col) {
        switch (col.gameObject.tag) {
            case "Enemy":
                // Collision with Drone
                if (col.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                    enemy.Damage(power);
                }
                break;
            /*case "Goal":
                Destroy(col.gameObject);
                break;*/
            default:
                if (col.gameObject.name.Equals("Platform")) return;
                break;
        }
        Destroy(gameObject);
    }
}

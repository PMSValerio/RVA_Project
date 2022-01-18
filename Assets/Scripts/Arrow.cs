using System.Security.Cryptography;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float speed = 32f;
    public int power = 50;

    float gravity = -0.005f;

    public Vector3 move = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        move = new Vector3(move.x,move.y+gravity,move.z);

        if(move != Vector3.zero) transform.rotation = Quaternion.LookRotation(move);
        transform.position += move;

        if (transform.position.y < -100) {
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
                // Collision with Drone's Eye
                else {
                    col.gameObject.GetComponentInParent<Enemy>().Damage(power);
                }
                break;
            case "Goal":
                Destroy(col.gameObject);
                break;
            default:
                break;
        }
    }
}

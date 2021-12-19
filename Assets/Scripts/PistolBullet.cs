using UnityEngine;

public class PistolBullet : MonoBehaviour {

    public float speed = 8f;
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
        if (col.gameObject.CompareTag("Enemy")) {
            Enemy en = col.gameObject.GetComponent<Enemy>();

            en.Damage(5);
            Destroy(gameObject);
        }
    }
}

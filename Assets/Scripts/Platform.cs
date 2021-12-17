using UnityEngine;

public class Platform : MonoBehaviour {
    [SerializeField] private float dst;
    private float src;

    private const float distance = 20;
    private const float speed = 2;

    private bool forward = true;

    public GameObject player;

    // Start is called before the first frame update
    private void Start() {
        src = transform.position.z;

        dst = src + distance;
    }

    // Update is called once per frame
    private void Update() {
        transform.position += new Vector3(0,0,(forward ? 1 : -1) * speed) * Time.deltaTime;

        if (transform.position.z >= dst) {
            transform.position = new Vector3(0, 0, dst);
            forward = false;
        }
        else if (transform.position.z <= src) {
            transform.position = new Vector3(0, 0, src);
            forward = true;
        }

        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }
}

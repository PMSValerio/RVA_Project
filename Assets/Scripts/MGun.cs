using UnityEngine;

public class MGun : Weapon {

    LineRenderer pointer;

    void Start() {
        ammoMax = 1000;
        ammo = ammoMax;

        pointer = transform.Find("Model/Pointer").gameObject.GetComponent<LineRenderer>();
        pointer.SetPosition(0,pointer.transform.InverseTransformPoint(transform.position));
    }

    // Update is called once per frame
    void Update() {
        var direction = transform.forward;
        RaycastHit hit;
        Vector3 og = pointer.GetPosition(0);
        Vector3 pointerEnd = new Vector3(og.x,og.y,300);

        bool rayHit = Physics.Raycast(transform.position + transform.forward, direction, out hit, 500);
        if (rayHit && !hit.collider.gameObject.CompareTag("Bullet")) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
            pointerEnd = new Vector3(og.x,og.y,pointerEnd.z);
        }

        if (ammo > 0 && action) {
            ammo--;
            Debug.DrawRay (transform.position, direction, Color.cyan, Time.deltaTime, false);
            if (rayHit) {
                switch (hit.collider.gameObject.tag) {
                    case "Enemy":
                        // Collision with Drone
                        if (hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy)) {
                            enemy.Damage(5);
                        } 
                        // Collision with Drone's Eye
                        else {
                            hit.collider.gameObject.GetComponentInParent<Enemy>().Damage(5);
                        }
                        break;
                    case "Goal":
                        Destroy(hit.collider.gameObject);
                        break;
                    default:
                        if (hit.collider.gameObject.name.Equals("Platform")) return;
                        break;
                }
            }
        }

        pointer.SetPosition(1,pointerEnd);
    }

    public override void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = pivot1;
        transform.localRotation = Quaternion.LookRotation(pivot2 - pivot1);
    }
}

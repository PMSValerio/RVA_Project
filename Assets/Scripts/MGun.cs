using UnityEngine;

public class MGun : Weapon {

    LineRenderer pointer;

    public override void Start() {
        base.Start();

        ammoMax = 1000;
        ResetAmmo();

        pointer = transform.Find("Pointer").gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        ApplyPose();
        
        pointer.SetPosition(0,transform.InverseTransformPoint(transform.position));

        var direction = transform.forward;
        RaycastHit hit;
        Vector3 pointerEnd = transform.InverseTransformPoint(transform.position + 300*transform.forward);

        bool rayHit = Physics.Raycast(transform.position, direction, out hit, 300);
        if (rayHit && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Obstacle") || hit.collider.gameObject.CompareTag("Goal"))) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
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

    public override void ApplyPose() {
        transform.localPosition = p1;
        transform.rotation = Quaternion.LookRotation(p2 - p1);
    }
}

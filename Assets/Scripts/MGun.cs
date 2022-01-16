using UnityEngine;

public class MGun : Weapon {

    LineRenderer pointer;

    void Start() {
        ammoMax = 1000;
        ammo = ammoMax;

        pointer = transform.Find("Model/Pointer").gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        var direction = transform.forward;
        RaycastHit hit;
        Vector3 pointerEnd = new Vector3(0,0,300);

        bool rayHit = Physics.Raycast(transform.TransformPoint(transform.position), direction, out hit, 300);
        if (rayHit) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
        }
        if (ammo >= 0 && action) {
            ammo--;
            Debug.DrawRay (transform.position, direction, Color.cyan, Time.deltaTime, false);
            if (rayHit) {

                if (hit.collider.gameObject.CompareTag("Enemy")) {
                    Enemy en = hit.collider.gameObject.GetComponent<Enemy>();

                    en.Damage(5);
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

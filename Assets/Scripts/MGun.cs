using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGun : Weapon
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (action) {
            var direction = 200*transform.TransformDirection(Vector3.forward);
            RaycastHit hit;

            Debug.DrawRay (transform.position, direction, Color.cyan, Time.deltaTime, false);
            if (Physics.Raycast(transform.position + transform.forward, direction, out hit, 300)) {

                if (hit.collider.gameObject.tag == "Enemy") {
                    Enemy en = hit.collider.gameObject.GetComponent<Enemy>();

                    en.Damage(5);
                }
            }
        }
    }

    public override void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = pivot1;
        transform.localRotation = Quaternion.LookRotation(pivot2 - pivot1);
    }
}

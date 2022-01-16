using System;
using UnityEngine;

public class Pistol : Weapon {
    public GameObject bulletPre;

    [SerializeField] private AudioSource shootAudio;

    private bool lastAction;
    public float cooldown = 2f;

    LineRenderer pointer;

    void Start() {
        ammoMax = 100;
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

        bool rayHit = Physics.Raycast(transform.position, direction, out hit, 300);
        if (rayHit && !hit.collider.gameObject.CompareTag("Bullet")) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
            pointerEnd = new Vector3(og.x,og.y,pointerEnd.z);
        }
        if (ammo >=0 && action && !lastAction) {
            ammo--;
            shootAudio.Play();
            GameObject bulletObj = Instantiate(bulletPre);
            bulletObj.transform.position = transform.position + transform.forward;
            bulletObj.transform.forward = transform.forward;
        }

        lastAction = action;

        pointer.SetPosition(1,pointerEnd);
    }
}

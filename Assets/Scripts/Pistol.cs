using System;
using UnityEngine;

public class Pistol : Weapon {
    public GameObject bulletPre;

    [SerializeField] private AudioSource shootAudio;

    private bool lastAction;
    public float cooldown = 2f;

    LineRenderer pointer;

    public override void Start() {
        base.Start();

        ammoMax = 100;
        ammo = ammoMax;

        pointer = transform.Find("Pointer").gameObject.GetComponent<LineRenderer>();
        pointer.SetPosition(0,transform.InverseTransformPoint(transform.position));
    }

    // Update is called once per frame
    void Update() {
        ApplyPose();
        
        var direction = transform.forward;
        RaycastHit hit;
        Vector3 pointerEnd = transform.InverseTransformPoint(transform.position + 300*transform.forward);

        bool rayHit = Physics.Raycast(transform.position, direction, out hit, 300);
        if (rayHit && (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Obstacle") || hit.collider.gameObject.CompareTag("Goal"))) {
            pointerEnd = transform.InverseTransformPoint(hit.point);
        }
        if (ammo > 0 && action && !lastAction) {
			bool flag = false;
            if (!GameManager.Instance.GetIsGamePaused()) {
                if (!GameManager.Instance.GetHasGameStarted()) {
                    flag = true;
                }
                else {
                    ammo--;
                }
            }

            if (!flag) {
                shootAudio.Play();
                GameObject bulletObj = Instantiate(bulletPre);
                bulletObj.transform.position = transform.position + transform.forward;
                bulletObj.transform.forward = transform.forward;
            }
        }

        lastAction = action;

        pointer.SetPosition(1,pointerEnd);
    }
}

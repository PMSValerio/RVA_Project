using System;
using UnityEngine;

public class Pistol : Weapon {
    public GameObject bulletPre;

    [SerializeField] private AudioSource shootAudio;

    private bool lastAction;
    public float cooldown = 2f;
    
    // Update is called once per frame
    void Update() {
        if (action && !lastAction) {
            shootAudio.Play();
            GameObject bulletObj = Instantiate(bulletPre);
            bulletObj.transform.position = transform.position + transform.forward;
            bulletObj.transform.forward = transform.forward;
        }

        lastAction = action;
    }
}

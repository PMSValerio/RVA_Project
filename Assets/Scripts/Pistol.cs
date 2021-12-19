using UnityEngine;

public class Pistol : Weapon {
    public GameObject bulletPre;

    public float cooldown = 2f;

    // Update is called once per frame
    void Update() {
        if (action) {
            GameObject bulletObj = Instantiate(bulletPre, transform.position + transform.forward, Quaternion.identity);
            bulletObj.transform.forward = transform.forward;
        }
    }
}

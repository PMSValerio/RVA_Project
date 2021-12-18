using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public GameObject bulletPre;

    public float cooldown = 2f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (action) {
            GameObject bulletObj = Instantiate(bulletPre);
            bulletObj.transform.position = transform.position + transform.forward;
            bulletObj.transform.forward = transform.forward;
        }
    }
}

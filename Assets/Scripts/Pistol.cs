using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public GameObject bulletPre;

    public GameObject cam;

    public float cooldown = 2f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Blam");
            GameObject bulletObj = Instantiate(bulletPre);
            bulletObj.transform.position = cam.transform.position + cam.transform.forward;
            bulletObj.transform.forward = cam.transform.forward;
        }
    }
}

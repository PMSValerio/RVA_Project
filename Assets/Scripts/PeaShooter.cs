using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : MonoBehaviour
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

    public void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        Debug.Log(transform.position);
        transform.position = pivot1;
        transform.localPosition = new Vector3(0,0,0);
        transform.rotation = rot1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    Camera cam;

    public GameObject[] weapons;
    private int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        foreach (GameObject weapon in weapons) {
            GameObject w = Instantiate(weapon);
            //w.transform.parent = transform;
        }
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cam.transform.rotation);
        Vector3 p1 = cam.transform.position + 0.5f*cam.transform.forward;
        Quaternion r1 = cam.transform.rotation;
        Vector3 p2 = cam.transform.position + cam.transform.forward;
        Quaternion r2 = cam.transform.rotation;

        weapons[current].GetComponent<PeaShooter>().Manipulate(p1,r1,p2,r2);
    }

    void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i;
        weapons[current].gameObject.SetActive(true);
    }
}

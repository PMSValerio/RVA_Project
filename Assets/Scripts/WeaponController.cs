using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    Camera cam;

    public GameObject[] weapons_prefab;
    private List<GameObject> weapons;
    private int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        weapons = new List<GameObject>();
        cam = Camera.main;
        foreach (GameObject weapon in weapons_prefab) {
            GameObject w = Instantiate(weapon);
            w.transform.parent = transform;
            weapons.Add(w);
        }
        SwitchWeapon(0);
        Debug.Log(weapons.Count);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p1 = cam.transform.position + cam.transform.forward + new Vector3(0.2f,-0.5f,0);
        Quaternion r1 = cam.transform.rotation;
        Vector3 p2 = cam.transform.position + 2*cam.transform.forward + new Vector3(0.2f,-0.5f,0);
        Quaternion r2 = cam.transform.rotation;

        weapons[current].GetComponent<Weapon>().Manipulate(p1,r1,p2,r2);
    }

    void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i;
        weapons[current].gameObject.SetActive(true);
    }
}

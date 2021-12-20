using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    private Camera cam;

    public GameObject[] weaponsPrefab;
    private List<GameObject> weapons;
    private int current;

    [SerializeField] private Platform platformWithAgent;

    // Start is called before the first frame update
    void Start() {
        weapons = new List<GameObject>();
        cam = Camera.main;
        foreach (GameObject weapon in weaponsPrefab) {
            GameObject w = Instantiate(weapon);
            //w.transform.parent = transform;
            weapons.Add(w);
        }
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update() {
        if (platformWithAgent.GetAgentSpeed() == 0 && Input.GetMouseButtonDown(0)) {
            platformWithAgent.SetAgentSpeed(3.5f);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            SwitchWeapon(current+1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            SwitchWeapon(current-1);
        }

        Vector3 p1 = cam.transform.position + cam.transform.forward + cam.transform.right*0.5f + cam.transform.up*(-0.4f);
        Quaternion r1 = cam.transform.rotation;
        Vector3 p2 = cam.transform.position + 2*cam.transform.forward + cam.transform.right*0.5f;
        Quaternion r2 = cam.transform.rotation;

        Weapon wep = weapons[current].GetComponent<Weapon>();
        wep.Manipulate(p1,r1,p2,r2);
        wep.action = Input.GetMouseButton(0);
    }

    void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i % weapons.Count;
        if (current<0) current*=-1;
        weapons[current].gameObject.SetActive(true);
    }
}

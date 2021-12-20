using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControllerOQ : MonoBehaviour {

    public GameObject[] weaponsPrefab;
    private List<GameObject> weapons;
    private int current;

    [SerializeField] private Platform platformWithAgent;
    
    // Start is called before the first frame update
    void Start() {
        weapons = new List<GameObject>();
        foreach (GameObject weapon in weaponsPrefab) {
            GameObject w = Instantiate(weapon);
            w.transform.parent = transform;
            weapons.Add(w);
        }
        SwitchWeapon(0);
    }

    // Update is called once per frame
    void Update() {

        if (platformWithAgent.GetAgentSpeed() == 0 && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)>0) {
            platformWithAgent.SetAgentSpeed(3.5f);
        }
        
        OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            SwitchWeapon(current+1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two)) {
            SwitchWeapon(current-1);
        }

        Vector3 p1, p2;
        Quaternion r1, r2;

        p1 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + transform.position;
        r1 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        
        p2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + transform.position;
        r2 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        Weapon wep = weapons[current].GetComponent<Weapon>();
        wep.Manipulate(p1,r1,p2,r2);

        wep.action = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)>0;
    }

    void SwitchWeapon(int i) {
        weapons[current].gameObject.SetActive(false);
        current = i % weapons.Count;
        if (current<0) current*=-1;
        weapons[current].gameObject.SetActive(true);
    }
}
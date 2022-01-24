using UnityEngine;

public class WeaponControllerOQ : ControllerParent {

    bool last_button;
    
    // Start is called before the first frame update
    private new void Start() {
        base.Start();
        last_button = false;
    }

    // Update is called once per frame
    private new void Update() {
        base.Update();
        
        OVRInput.Update();
        //transform.Find("OVRCameraRig").position = transform.position;
        if (Time.timeScale == 0) return;
        if (!last_button && OVRInput.Get(OVRInput.Button.Two)) {
            SwitchWeapon(current+1);
            last_button = true;
        }
        else if (!last_button && OVRInput.Get(OVRInput.Button.One)) {
            SwitchWeapon(current-1);
            last_button = true;
        }
        else {
            last_button = false;
        }

        Vector3 p1 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + transform.position;
        Quaternion r1 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 p2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + transform.position;
        Quaternion r2 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        Weapon wep = weapons[current].GetComponent<Weapon>();
        if (GameManager.Instance.righthand) {
            wep.Manipulate(transform.InverseTransformPoint(p1),r1,transform.InverseTransformPoint(p2),r2);
        }
        else {
            wep.Manipulate(transform.InverseTransformPoint(p2),r2,transform.InverseTransformPoint(p1),r1);
        }
        wep.action = canAction && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)>0;
    }
}
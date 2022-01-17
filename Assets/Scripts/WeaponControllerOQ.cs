using UnityEngine;

public class WeaponControllerOQ : ControllerParent {
    
    // Start is called before the first frame update
    private new void Start() {
        base.Start();
    }

    // Update is called once per frame
    private new void Update() {
        base.Update();
        
        OVRInput.Update();
        if (Time.timeScale == 0) return;
        if (OVRInput.GetDown(OVRInput.Button.Two)) {
            SwitchWeapon(current+1);
        }
        else if (OVRInput.GetDown(OVRInput.Button.One)) {
            SwitchWeapon(current-1);
        }

        Vector3 p1 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + transform.position;
        Quaternion r1 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 p2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + transform.position;
        Quaternion r2 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        Weapon wep = weapons[current].GetComponent<Weapon>();
        wep.Manipulate(p1,r1,p2,r2);
        wep.action = canAction && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)>0;
    }
}
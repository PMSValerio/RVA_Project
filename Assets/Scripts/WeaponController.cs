using UnityEngine;

public class WeaponController : ControllerParent {
    private Camera cam;
    
    // Start is called before the first frame update
    private new void Start() {
        base.Start();
        cam = Camera.main;
    }

    // Update is called once per frame
    private new void Update() {
        base.Update();
        if (Time.timeScale == 0) return;
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            SwitchWeapon(current+1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            SwitchWeapon(current-1);
        }

        Vector3 p1 = cam.transform.position + cam.transform.forward + cam.transform.right*0.5f + cam.transform.up*(-0.4f);
        Quaternion r1 = cam.transform.rotation;
        Vector3 p2 = cam.transform.position + 2*cam.transform.forward + cam.transform.right*0.5f + cam.transform.up*(-0.4f);
        Quaternion r2 = cam.transform.rotation;

        Weapon wep = weapons[current].GetComponent<Weapon>();
        wep.Manipulate(p1,r1,p2,r2);
        wep.action = canAction && Input.GetMouseButton(0);
    }
}

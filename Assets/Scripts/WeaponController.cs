using UnityEngine;

public class WeaponController : ControllerParent {
    private Camera cam;
    private float acCounter; // used only to test bow weapon
    
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
        wep.action = canAction && Input.GetMouseButton(0);

        if (wep is Bow) {
            if (wep.action) {
                acCounter += 0.01f;
                p1 = cam.transform.position + 0.8f*cam.transform.forward + (-cam.transform.forward*acCounter) + cam.transform.right*0.5f + cam.transform.up*(-0.4f);
                p2 = cam.transform.position + cam.transform.forward + cam.transform.right*0.5f + cam.transform.up*(-0.4f);
            }
            else {
                acCounter = 0;
                p2 = cam.transform.position + cam.transform.forward + cam.transform.right*(-0.2f) + cam.transform.up*(-0.4f);
            }
        }
        else acCounter = 0;

        p1 = weapons[current].transform.parent.InverseTransformPoint(p1);
        p2 = weapons[current].transform.parent.InverseTransformPoint(p2);
        if (GameManager.Instance.righthand) {
            wep.Manipulate(p1,r1,p2,r2);
        }
        else {
            wep.Manipulate(p2,r2,p1,r1);
        }
    }
}

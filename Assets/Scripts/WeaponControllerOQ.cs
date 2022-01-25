using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponControllerOQ : ControllerParent {

    bool last_button;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;
    
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

        bool fourPressed = OVRInput.Get(GameManager.Instance.righthand ? OVRInput.Button.Two : OVRInput.Button.Four);
        bool threePressed = OVRInput.Get(GameManager.Instance.righthand ? OVRInput.Button.One : OVRInput.Button.Three);
        
        if (!last_button && fourPressed) {
            SwitchWeapon(current+1);
        }
        else if (!last_button && threePressed) {
            SwitchWeapon(current-1);
        }

        last_button = fourPressed || threePressed;

        Vector3 p1 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + transform.position;
        Quaternion r1 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 p2 = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + transform.position;
        Quaternion r2 = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        Weapon wep = weapons[current].GetComponent<Weapon>();
        if (GameManager.Instance.righthand) {
            wep.Manipulate(transform.InverseTransformPoint(p1+transform.up),r1,transform.InverseTransformPoint(p2+transform.up),r2);
        }
        else {
            wep.Manipulate(transform.InverseTransformPoint(p2+transform.up),r2,transform.InverseTransformPoint(p1+transform.up),r1);
        }
        wep.action = canAction && OVRInput.Get( GameManager.Instance.righthand?OVRInput.Axis1D.SecondaryIndexTrigger:OVRInput.Axis1D.PrimaryIndexTrigger )>0;
    }
}
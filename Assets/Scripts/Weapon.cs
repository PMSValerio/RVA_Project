using UnityEngine;

public class Weapon : MonoBehaviour {

    public int ammoMax;
    public int ammo;

    public bool action;

    public virtual void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = pivot1;
        transform.localRotation = rot1;
    }
}

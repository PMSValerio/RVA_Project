using UnityEngine;

public class Weapon : MonoBehaviour {

    public int ammoMax;
    public int ammo;

    public bool acquired;

    public bool action;

    public bool selected = false;

    protected Vector3 p1;
    protected Vector3 p2;
    protected Quaternion r1;
    protected Quaternion r2;

    public virtual void Start() {
        acquired = false;
        if (!selected) gameObject.SetActive(false);
    }

    public virtual void ApplyPose() {
        //transform.position = p1;
        transform.localPosition = p1;
        transform.rotation = r1;
    }

    public virtual void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = GameManager.Instance.Player.transform.position;
        p1 = pivot1;
        r1 = rot1;
        p2 = pivot2;
        r2 = rot2;
    }

    public virtual bool AddAmmo(int x) {
        ammo += x;
        if (ammo > ammoMax) ammo = ammoMax;

        if (!acquired && x>0) {
            acquired = true;
            return true;
        }
        return false;
    }
}

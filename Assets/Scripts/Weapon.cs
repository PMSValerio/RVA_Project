using UnityEngine;

public class Weapon : MonoBehaviour {

    public int ammoMax;
    public int ammo;

    public bool action;

    public bool selected = false;

    protected Vector3 p1;
    protected Vector3 p2;
    protected Quaternion r1;
    protected Quaternion r2;

    public virtual void Start() {
        if (!selected) gameObject.SetActive(false);
    }

    public virtual void ApplyPose() {
        transform.localPosition = p1;
        transform.localRotation = r1;
    }

    public virtual void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = GameManager.Instance.Player.transform.position;
        p1 = pivot1;
        r1 = rot1;
        p2 = pivot2;
        r2 = rot2;
    }
}

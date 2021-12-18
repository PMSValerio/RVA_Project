using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void Manipulate(Vector3 pivot1, Quaternion rot1, Vector3 pivot2, Quaternion rot2) {
        transform.position = pivot1;
        transform.localRotation = rot1;
    }
}

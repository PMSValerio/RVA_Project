using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Camera camera;

    public float minX = -360F;
    public float maxX = 360F;
    public float minY = -60F;
    public float maxY = 60F;

    float speedX = 5F;
    float speedY = 2F;

    float rotX = 0F;
    float rotY = 0F;
    Quaternion ogRot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ogRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        rotX += Input.GetAxis("Mouse X") * speedX;
        rotY += Input.GetAxis("Mouse Y") * speedY;
        
        rotY = Mathf.Clamp(rotY, minY, maxY);

        Quaternion xQuat = Quaternion.AngleAxis(rotX, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotY, -Vector3.right);

        camera.transform.rotation = ogRot * xQuat * yQuat;
        
    }
}

using UnityEngine;

public class MouseLook : MonoBehaviour {
    [SerializeField] private float minY = -60F;
    [SerializeField] private float maxY = 60F;

    private Camera lookCamera;

    private const float speedX = 5F;
    private const float speedY = 2F;

    private float rotX;
    private float rotY;
    private Quaternion ogRot;

    // Start is called before the first frame update
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        ogRot = transform.localRotation;
        lookCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update() {
        rotX += Input.GetAxis("Mouse X") * speedX;
        rotY += Input.GetAxis("Mouse Y") * speedY;
        
        rotY = Mathf.Clamp(rotY, minY, maxY);

        Quaternion xQuat = Quaternion.AngleAxis(rotX, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotY, -Vector3.right);

        lookCamera.transform.rotation = ogRot * xQuat * yQuat;
    }
}

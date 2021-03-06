using UnityEngine;

public class CustMouseLook : MonoBehaviour {
    // MouseLook attributes
    private const float speedX = 5F; // Mouse horizontal speed
    private const float speedY = 2F; // Mouse vertical speed
    [SerializeField] private float minY = -60F; // Minimum Y allowed
    [SerializeField] private float maxY = 60F; // Maximum Y allowed
    private float rotX; // Mouse horizontal rotation
    private float rotY; // Mouse vertical rotation
    private Quaternion originalRotation; // Mouse original local rotation
    private float ProtX; // Mouse horizontal rotation
    private float ProtY; // Mouse vertical rotation

    // Main Camera reference
    private Camera mainCamera;

    // Start is called before the first frame update
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update() {
        if (GameManager.Instance.GetIsGamePaused()) {
            // Mouse input rotation
            ProtX += Input.GetAxis("Mouse X") * speedX;
            ProtY += Input.GetAxis("Mouse Y") * speedY;
        
            // Limit rotation to allowed values
            ProtY = Mathf.Clamp(ProtY, minY, maxY);

            // Rotate camera according to mouse's movement
            Quaternion PxQuat = Quaternion.AngleAxis(ProtX, Vector3.up);
            Quaternion PyQuat = Quaternion.AngleAxis(ProtY, -Vector3.right);
            mainCamera.transform.rotation = new Quaternion(0, 0, 0, 1) * PxQuat * PyQuat;
        }
        else {
            ProtX = 0f;
            ProtY = 0f;
            // Mouse input rotation
            rotX += Input.GetAxis("Mouse X") * speedX;
            rotY += Input.GetAxis("Mouse Y") * speedY;
        
            // Limit rotation to allowed values
            rotY = Mathf.Clamp(rotY, minY, maxY);

            // Rotate camera according to mouse's movement
            Quaternion xQuat = Quaternion.AngleAxis(rotX, Vector3.up);
            Quaternion yQuat = Quaternion.AngleAxis(rotY, -Vector3.right);
            mainCamera.transform.rotation = originalRotation * xQuat * yQuat;
        }
    }
}
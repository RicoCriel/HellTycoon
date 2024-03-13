using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MoveSpeed = 2;
    public float minZoom = 20f;
    public float maxZoom = 60f;
    public float zoomSpeed = 5f; // Adjust this to control zoom speed

    [SerializeField]
    private GameObject Parent;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera zoom controls
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = cam.fieldOfView - scroll * zoomSpeed;
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        cam.fieldOfView = newZoom;

        // Camera movement controls
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= Vector3.right;
        }

        // Convert movement to world space
        moveDirection = Parent.transform.TransformDirection(moveDirection);

        
        Parent.transform.position += moveDirection.normalized * MoveSpeed * Time.deltaTime;
    }
}

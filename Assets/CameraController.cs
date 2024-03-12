using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    private Vector3 dragOrigin;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera zoom controls
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = transform.position + transform.forward * scroll * 10f;

        zoom.x = Mathf.Clamp(zoom.x, -maxZoom, maxZoom);
        zoom.z = Mathf.Clamp(zoom.z, -maxZoom, maxZoom);
        zoom.y = Mathf.Clamp(zoom.y, minZoom, maxZoom);

        transform.position = zoom;
        
        // Camera movement controls
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        transform.Translate(move, Space.World);
        // End of camera movement controls

        
    }
}

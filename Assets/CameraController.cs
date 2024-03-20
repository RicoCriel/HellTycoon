using UnityEngine;

public class CameraController : MonoBehaviour
{
   

    [SerializeField] private GameObject _parent;

    private Camera _cam;

    public float MoveSpeed = 2;
    public float MinZoom = 20f;
    public float MaxZoom = 60f;
    public float ZoomSpeed = 5f; // Adjust this to control zoom speed

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera zoom controls
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newZoom = - scroll * ZoomSpeed;
        //newZoom = Mathf.Clamp(newZoom, MinZoom, MaxZoom);
        //_cam.fieldOfView = newZoom;
        _cam.orthographicSize += newZoom;

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
        moveDirection = _parent.transform.TransformDirection(moveDirection);

        
        _parent.transform.position += moveDirection.normalized * MoveSpeed * Time.deltaTime;
    }
}

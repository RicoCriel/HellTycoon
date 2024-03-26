using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Space]
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _rotateSpeed = 50f;

    [Header("Dragging")]
    [SerializeField] private bool _drag = true;
    [SerializeField] private float _dragSpeed = 2f;

    [Header("Edge scrolling")]
    [SerializeField] private bool _edgeScrolling = false;
    [SerializeField] private int _edgeScrollSize = 20;

    [Header("Zoom")]
    [SerializeField] private float _zoomSpeed = 20f;

    private bool _dragPanMove = false;
    private Vector2 _lastMousePos = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleCameraZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z += 1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z -= 1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x += 1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x -= 1f;

        if (_edgeScrolling)
            HandleEdgeScrolling(ref inputDir);

        if (_drag)
            HandleDrag(ref inputDir);

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * _moveSpeed * Time.deltaTime;
    }

    private void HandleEdgeScrolling(ref Vector3 inputDir)
    {
        if (Input.mousePosition.x < _edgeScrollSize) inputDir.x -= 1f;
        if (Input.mousePosition.y < _edgeScrollSize) inputDir.z -= 1f;
        if (Input.mousePosition.x > Screen.width - _edgeScrollSize) inputDir.x += 1f;
        if (Input.mousePosition.y > Screen.height - _edgeScrollSize) inputDir.z += 1f;
    }

    private void HandleDrag(ref Vector3 inputDir)
    {
        if (Input.GetMouseButtonDown(1))
        {
            _dragPanMove = true;
            _lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _dragPanMove = false;
        }

        if (_dragPanMove)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - _lastMousePos;

            inputDir.x = -mouseMovementDelta.x * _dragSpeed;
            inputDir.z = -mouseMovementDelta.y * _dragSpeed;

            _lastMousePos = Input.mousePosition;
        }

    }

    private void HandleRotation()
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir += 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir -= 1f;

        transform.eulerAngles += new Vector3(0f, rotateDir * _rotateSpeed * Time.deltaTime, 0f);
    }

    private void HandleCameraZoom()
    {
        _camera.m_Lens.FieldOfView -= Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
    }
}



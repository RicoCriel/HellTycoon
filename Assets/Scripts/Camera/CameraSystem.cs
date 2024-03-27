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
    [SerializeField] private float _zoomSmoothing = 5f;
    [SerializeField] private bool _zoomFOV = false;
    [SerializeField] private bool _zoomFollowOffset = false;
    [SerializeField] private bool _zoomLowerY = true;

    [Header("FOV")]
    [SerializeField] private float _targetFieldOfView = 50f;
    [SerializeField] private float _minFieldOfView = 10f;
    [SerializeField] private float _maxFieldOfView = 100f;

    [Header("Follow Offset")]
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private float _minFollowOffset = 2;
    [SerializeField] private float _maxFollowOffset = 20;

    [Header("Lower Y")] 
    [SerializeField] private float _minY = 5;
    [SerializeField] private float _maxY = 40f;
    

    private bool _dragPanMove = false;
    private Vector2 _lastMousePos = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        HandleRotation();

        if (_zoomFOV)
            HandleCameraZoomFOV();

        if (_zoomFollowOffset)
            HandleZoomMoveForward();

        if(_zoomLowerY)
            HandleZoomLowerY();
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

    private void HandleCameraZoomFOV()
    {
        _targetFieldOfView -= Input.mouseScrollDelta.y * _zoomSpeed;

        _targetFieldOfView = Mathf.Clamp(_targetFieldOfView, _minFieldOfView, _maxFieldOfView);

        _camera.m_Lens.FieldOfView =
            Mathf.Lerp(_camera.m_Lens.FieldOfView, _targetFieldOfView, Time.deltaTime * _zoomSmoothing);
    }

    private void HandleZoomMoveForward()
    {
        Vector3 zoomDir = _followOffset.normalized;

        _followOffset -= zoomDir * Input.mouseScrollDelta.y * _zoomSpeed;

        if (_followOffset.sqrMagnitude < _minFollowOffset * _minFollowOffset)
        {
            _followOffset = zoomDir * _minFollowOffset;
        }
        else if (_followOffset.sqrMagnitude > _maxFollowOffset * _maxFollowOffset)
        {
            _followOffset = zoomDir * _maxFollowOffset;
        }

        var transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset =
            Vector3.Lerp(transposer.m_FollowOffset, _followOffset, Time.deltaTime * _zoomSmoothing);
    }

    private void HandleZoomLowerY()
    {
        _followOffset.y -= Input.mouseScrollDelta.y * _zoomSpeed;

        _followOffset.y = Mathf.Clamp(_followOffset.y, _minY, _maxY);

        var transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset =
            Vector3.Lerp(transposer.m_FollowOffset, _followOffset, Time.deltaTime * _zoomSmoothing);
    }
}



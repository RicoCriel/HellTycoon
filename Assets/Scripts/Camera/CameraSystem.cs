using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    [Space(20)]
    [Range(1, 100)]
    [SerializeField] private float _moveSpeed = 50f;
    [Range(1, 250)]
    [SerializeField] private float _rotateSpeed = 50f;

    [Header("Dragging")]
    [SerializeField] private bool _drag = true;
    [Range(0f, 1.5f)]
    [SerializeField] private float _dragSpeed = 2f;

    [Header("Edge scrolling")]
    [SerializeField] private bool _edgeScrolling = false;
    [Range(1, 50)]
    [SerializeField] private int _edgeScrollSize = 20;

    [Header("Zoom")]
    [Range(0, 20)]
    [SerializeField] private float _zoomSpeed = 20f;
    [Range(0, 20)]
    [SerializeField] private float _zoomSmoothing = 5f;

    [Space(10)]
    [SerializeField] private ZoomType _zoomType;

    [Header("FOV")]
    [SerializeField] private float _targetFieldOfView = 50f;
    [Range(0, 50)]
    [SerializeField] private float _minFieldOfView = 10f;
    [Range(30, 180)]
    [SerializeField] private float _maxFieldOfView = 100f;

    [Header("Follow Offset")]
    [SerializeField] private Vector3 _followOffset;
    [Range(0, 30)]
    [SerializeField] private float _minFollowOffset = 2;
    [Range(5, 50)]
    [SerializeField] private float _maxFollowOffset = 20;
    [Range(0.1f, 5f)]
    [SerializeField] private float _minFollowY = 0.5f;

    [Header("Lower Y")]
    [Range(0, 50)]
    [SerializeField] private float _minY = 5;
    [Range(1, 200)]
    [SerializeField] private float _maxY = 40f;


    private bool _dragPanMove = false;
    private Vector2 _lastMousePos = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        HandleRotation();


        switch (_zoomType)
        {
            case ZoomType.FOV:
                HandleCameraZoomFOV();
                break;
            case ZoomType.MoveForward:
                HandleZoomMoveForward();
                break;
            case ZoomType.LowerY:
                HandleZoomLowerY();
                break;
            case ZoomType.Combined:
                HandleZoomCombined();
                break;
        }
        //if (_zoomFOV)
        //    HandleCameraZoomFOV();

        //else if (_zoomFollowOffset)
        //    HandleZoomMoveForward();

        //else if (_zoomLowerY)
        //    HandleZoomLowerY();

        //else if(_zoomCombinedMethod)
        //    HandleZoomCombined();
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

        var newOffset = _followOffset;

        newOffset -= zoomDir * Input.mouseScrollDelta.y * _zoomSpeed;

        if (newOffset.y <= _minFollowY) return;

        _followOffset = newOffset;

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

    private void HandleZoomCombined()
    {
        if (_followOffset.y > _minY)
        {
            HandleZoomLowerY();
        }
        else
        {
            HandleZoomMoveForward();
        }
    }
}

public enum ZoomType
{
    FOV,
    MoveForward,
    LowerY,
    Combined
}



using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Economy;

namespace FreeBuild
{
    public class FreeBuildManager : MonoBehaviour
    {
        [SerializeField] private Material _transParentMaterial;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private PortalManager _portalManager;
        [SerializeField] private LandLayerManager _landLayerManager;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private MachineManager _machineManager;
        [SerializeField] private string _inputTag;
        [SerializeField] private string _outputTag;
        [SerializeField] private GameObject _ghostObjectPrefab;
        [SerializeField] private Material _goodMaterial;
        [SerializeField] private Material _badMaterial;
        [SerializeField] private EconomyManager _economyManager;
        [SerializeField] private int _buildingLayer;
        [SerializeField] private LayerMask _snapLayerMask;
        [SerializeField] private float _snapThreshold = 5.5f;

        private string _buildTag;
        private GameObject _ghostObject;
        private GameObject _ghostObject2;

        private GameObject _realObject;
        private bool _locked = false;
        private bool _isSnapped = false;
        private bool _canBuild = false;
        private int _currentCost = 0;
        private Vector3 _2ghostOffset;
        private bool _startTracking = false;
        private Vector3 _startPosition;
        private float _trackedDist = 0f;
        private float _maxDist = 150f;

        // Rotation speed
        public float RotateSpeed = 100.0f;
        // Height change speed
        public float HeightChangeSpeed = 1.0f;

        public static event Action OnStartSnapping;


        private void Awake()
        {
            BuildingPanelUI._onPartChosen += CreateGhostObject;

            if (_landLayerManager == null)
            {
                _landLayerManager = FindObjectOfType<LandLayerManager>();
            }
        }

        void OnEnable()
        {
            FreeBuildManager.OnStartSnapping += OnStartSnapping;
        }

        void OnDisable()
        {
            FreeBuildManager.OnStartSnapping -= OnStartSnapping;
        }

        public void StartTrackingMouse()
        {
            _startTracking = true;
            _startPosition = Input.mousePosition;
            _trackedDist = 0f;
        }

        public void LockObj()
        {
            _isSnapped = true;
        }

        public void SetLocked(bool locked)
        {
            _locked = locked;
        }

        public void CreateGhostObject(BuildingData data)
        {
            _realObject = data.Prefab;
            _buildTag = data.BuildTag;
            _currentCost = data.Price;
            if (null == _realObject)
            {
                Debug.LogError("You have to list the objects you're trying to build on.");
                return;
            }
            if (null == _realObject.GetComponent<FreeBuildObject>())
            {
                Debug.LogError("The object you are trying to build must have a ConstructionObject Component.");
                return;
            }

            if (_ghostObject)
                Destroy(_ghostObject);

            //
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit);

            if (isHit)
            {
                InstantiateGhostObject(hit);
            }
        }


        void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && _ghostObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Build();
                }

                if (!_locked)
                {
                    if (!_isSnapped)
                    {
                        // Move
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        bool isHit = Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer);

                        if (isHit && _ghostObject)
                        {
                            MoveGhostObject(hit);
                        }
                    }
                }

            }

            // Rotation
            if (Input.GetKey(KeyCode.R))
            {
                RotateGhostObject(RotateSpeed * Time.deltaTime);
            }

            Snapper ghostSnapper = _ghostObject ? _ghostObject.GetComponent<Snapper>() : null;
            if (_ghostObject && !_locked && ghostSnapper && !ghostSnapper.IsPlaced)
            {
                AttemptSnapping();
            }


            if (_startTracking)
            {
                // Calculate the distance moved by the mouse since last frame
                Vector3 currentPosition = Input.mousePosition;
                float distanceMoved = Vector3.Distance(currentPosition, _startPosition);

                // Add the distance moved to the total tracked distance
                _trackedDist += distanceMoved;

                // Update the start position for the next frame
                _startPosition = currentPosition;

                Debug.Log(_trackedDist);

                if (_trackedDist >= _maxDist)
                {
                    _startTracking = false;
                    _isSnapped = false;
                    _locked = false;
                    Debug.Log("Max distance reached!");
                }
            }
        }




        void AttemptSnapping()
        {
            Snapper ghostSnapper = _ghostObject.GetComponent<Snapper>();
            if (ghostSnapper == null || ghostSnapper.IsPlaced) return; // Added check for isPlaced

            foreach (Transform ghostSnapPoint in ghostSnapper.SnapPoints)
            {
                Collider[] hitColliders = Physics.OverlapSphere(ghostSnapPoint.position, _snapThreshold, _snapLayerMask);
                if (hitColliders.Length <= 0) { return; }
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject == _ghostObject)
                    {
                        continue; // Skip this iteration, don't consider the ghost object for snapping to itself
                    }
                    Snapper targetSnapper = hitCollider.transform.GetComponent<Snapper>();
                    if (targetSnapper != null && targetSnapper.IsPlaced)
                    { // Ensure target isn't already placed
                        foreach (Transform targetSnapPoint in targetSnapper.SnapPoints)
                        {
                            float distance = Vector3.Distance(ghostSnapPoint.position, targetSnapPoint.position);
                            if (distance <= _snapThreshold)
                            {
                                OnStartSnapping?.Invoke();
                                SnapObject(ghostSnapPoint, targetSnapPoint, ghostSnapper);
                                return; // Snap to the first match found for simplicity
                            }
                        }
                    }

                }
            }
        }

        void SnapObject(Transform ghostSnapPoint, Transform targetSnapPoint, Snapper ghostSnapper)
        {
            if (!_isSnapped)
            {
                // Calculate offset and rotation required to align the ghost object's snap point with the target snap point
                Vector3 positionOffset = targetSnapPoint.position - ghostSnapPoint.position;
                _ghostObject.transform.position += positionOffset;

                // Mark the ghost object as placed to prevent further snapping
                //ghostSnapper.IsPlaced = true;

                _isSnapped = true; // Prevent further movement until manual unlock or placement
                StartTrackingMouse();
            }

        }

        private void MoveGhostObject(RaycastHit hit)
        {


                _ghostObject.transform.position = new Vector3(hit.point.x, hit.point.y + 3 /*+ GetObjectHeight(hit.transform)*/, hit.point.z);
                if (_realObject.GetComponent<DemonPortal>() != null && _ghostObject2 != null)

                {
                    _ghostObject2.transform.position = new Vector3(hit.point.x - (_2ghostOffset.x/3), hit.point.y + 3 +  _2ghostOffset.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z + _2ghostOffset.z);
                }


                _canBuild = hit.transform.gameObject.transform.gameObject.tag == _buildTag;
                CheckForCollision();
                SetGhostOutline(hit.transform.gameObject);
        }

        private void SetGhostOutline(GameObject areaToBeBuilt)
        {
            var material = _canBuild ? _goodMaterial : _badMaterial;
            _ghostObject.GetComponent<Renderer>().material = material;
        }

        private void InstantiateGhostObject(RaycastHit hit)
        {
            float heightOffset = 3f;
            _ghostObject = Instantiate(_ghostObjectPrefab, new Vector3(hit.point.x, hit.point.y + heightOffset, hit.point.z),
                Quaternion.identity);

            var meshFilter = _realObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = _realObject.GetComponentInChildren<MeshFilter>();
            }

            _ghostObject.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
            _ghostObject.transform.localScale = meshFilter.transform.lossyScale;

            if (_realObject.GetComponent<DemonPortal>() != null)
            {
                Transform curr = _landLayerManager.GetCurrPlot().transform;
                Transform next;
                if (_landLayerManager.NextPlot(curr.gameObject) != null)
                {
                    next = _landLayerManager.NextPlot(curr.gameObject).transform;
                    _2ghostOffset.x = next.position.x;
                    _2ghostOffset.y = next.position.y;
                    _2ghostOffset.z = next.position.z;
                    _ghostObject2 = Instantiate(_ghostObjectPrefab,
                        new Vector3(hit.point.x + next.position.x, hit.point.y + heightOffset,
                            hit.point.z + next.position.z), Quaternion.identity);
                    _ghostObject2.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
                    _ghostObject2.transform.localScale = meshFilter.transform.lossyScale;
                }


            }

            SetGhostOutline(hit.transform.gameObject);
            _canBuild = hit.transform.gameObject.transform.gameObject.tag == _buildTag;
        }

        private void RotateGhostObject(float angle)
        {
            if (_ghostObject)
            {
                _ghostObject.transform.Rotate(0, angle, 0);
            }
        }

        private void CheckForCollision()
        {
            if (_ghostObject.GetComponent<DemonPortal>() != null)
            {
                if (_ghostObject2.GetComponent<Snapper>().IsColliding)
                {
                    _canBuild = false;
                }
            }
            if (_ghostObject.GetComponent<Snapper>() != null)
            {
                if (_ghostObject.GetComponent<Snapper>().IsColliding)
                {
                    _canBuild = false;
                }
            }
        }

        public void Build()
        {
            CheckForCollision();

            if (_canBuild)
            {
                if (_economyManager != null)
                {
                    if (!_economyManager.BuyObject(_currentCost)) return;

                    _currentCost = 0;
                }

               

                if (_realObject.GetComponent<DemonPortal>() != null)
                {
                    BuildPortal();
                }
                else
                {
                    GameObject go = Instantiate(_realObject, _ghostObject.transform.position, _ghostObject.transform.rotation);
                    Snapper snapper = go.GetComponent<Snapper>();
                    if (_rootObject)
                        go.transform.SetParent(_rootObject.transform);

                    //go.layer = _buildingLayer;
                    if (snapper != null)
                    {
                        snapper.IsPlaced = true; // Mark the real object as placed
                    }

                    if (go.GetComponent<Snapper>() != null)
                    {
                        go.GetComponent<Snapper>().IsPlaced = true;
                    }
                }


                DestroyGhostObject();
                _locked = false;
            }
            else
            {
                Debug.LogWarning("Ya can't build in an impossible area matey!");
            }
        }

        private void BuildPortal()
        {
            Transform curr = _landLayerManager.GetCurrPlot().transform;
            Transform next;
            if (_landLayerManager.NextPlot(curr.gameObject) != null)
            {
                next = _landLayerManager.NextPlot(curr.gameObject).transform;
            }
            else
            {
                next = null;
            }
            _ghostObject.transform.SetParent(curr);
            if (next != null)
            {
                _portalManager.PlacePortal(_ghostObject.transform.localPosition, _ghostObject2.transform.localPosition, curr, next);


            }
        }

        public void CancelBuilding()
        {
            DestroyGhostObject();
        }

        private float GetObjectHeight(Transform tf)
        {
            if (tf.GetComponent<Collider>())
            {
                return tf.GetComponent<Collider>().bounds.size.y / 2;
            }
            else
            {
                return tf.transform.position.y;
            }
        }

        private void DestroyGhostObject()
        {
            if (_ghostObject)
            {
                Destroy(_ghostObject);
            }

            if (_ghostObject2)
            {
                Destroy(_ghostObject2);
                _ghostObject2 = null;
            }
        }

    }
}



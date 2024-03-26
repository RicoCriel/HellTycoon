using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace FreeBuild
{
    public class FreeBuildManager : MonoBehaviour
    {


        // can select outline Color.
        //public Color AbleAreaColor = new Color(0, 255, 0);
        //public Color NotAbleAreaColor = new Color(255, 0, 0);
        public static bool DestructionMode = false;

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
        [SerializeField] private EconManager _econManager;
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
        private float _2ghostOffset;

        // Rotation speed
        public float RotateSpeed = 100.0f;
        // Height change speed
        public float HeightChangeSpeed = 1.0f;

        public static event Action OnStartSnapping;


        private void Awake()
        {
            //BuildingPanelUI._onPartChosen += CreateGhostObject;
            BuildSideUI._onBuild += CreateGhostObject;
        }


        void OnEnable()
        {
            FreeBuildManager.OnStartSnapping += OnStartSnapping;
        }

        void OnDisable()
        {
            FreeBuildManager.OnStartSnapping -= OnStartSnapping;
        }


        public void LockObj()
        {
            _isSnapped = true;
        }

        public static void ToggleDestruct()
        {
            DestructionMode = !DestructionMode;
        }

        public void SetLocked(bool locked)
        { _locked = locked; }

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
                CreateGhostObject(hit);
            }
        }


        void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Height change
                float heightChange = 0.0f;
                if (Input.GetKey(KeyCode.Space))
                {
                    heightChange += 1.0f;
                }
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    heightChange -= 1.0f;
                }

                if (_ghostObject)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _locked = !_locked;
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

            }

            // Rotation
            if (Input.GetKey(KeyCode.Q))
            {
                RotateGhostObject(-RotateSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateGhostObject(RotateSpeed * Time.deltaTime);
            }
            if (Input.GetMouseButtonDown(1))
            {
                _isSnapped = false;
            }

            Snapper ghostSnapper = _ghostObject ? _ghostObject.GetComponent<Snapper>() : null;
            if (_ghostObject && !_locked && ghostSnapper && !ghostSnapper.IsPlaced)
            {
                AttemptSnapping();
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
            // Calculate offset and rotation required to align the ghost object's snap point with the target snap point
            Vector3 positionOffset = targetSnapPoint.position - ghostSnapPoint.position;
            _ghostObject.transform.position += positionOffset;

            // Mark the ghost object as placed to prevent further snapping
            //ghostSnapper.IsPlaced = true;

            _isSnapped = true; // Prevent further movement until manual unlock or placement
        }

        private void MoveGhostObject(RaycastHit hit)
        {
            _ghostObject.transform.position = new Vector3(hit.point.x, hit.point.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z);
            if (_realObject.GetComponent<DemonPortal>() != null)
            {
                    _ghostObject2.transform.position = new Vector3(hit.point.x + _2ghostOffset, hit.point.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z);
            }
                
                
            SetGhostOutline(hit.transform.gameObject);
            _canBuild = hit.transform.gameObject.transform.gameObject.tag == _buildTag;
        }

        private void SetGhostOutline(GameObject areaToBeBuilt)
        {
            var material = _canBuild ? _goodMaterial : _badMaterial;
            _ghostObject.GetComponent<Renderer>().material = material;
        }

        private void CreateGhostObject(RaycastHit hit)
        {
            _ghostObject = Instantiate(_ghostObjectPrefab, new Vector3(hit.point.x, hit.point.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z), Quaternion.identity);


            if (_realObject.GetComponent<DemonPortal>() != null)
            {
                Transform curr = _landLayerManager.GetCurrPlot().transform;
                Transform next;
                if (_landLayerManager.NextPlot(curr.gameObject) != null)
                {
                    next = _landLayerManager.NextPlot(curr.gameObject).transform;
                    _2ghostOffset = next.position.x;
                    _ghostObject2 = Instantiate(_ghostObjectPrefab, new Vector3(hit.point.x + next.position.x, hit.point.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z), Quaternion.identity);

                }
            }

            var meshFilter = _realObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = _realObject.GetComponentInChildren<MeshFilter>();
            }
            //Snapper snapper = _realObject.GetComponent<Snapper>();
            //if (snapper != null)
            //{
            //    var ghostSnap = _ghostObject.AddComponent<Snapper>();
            //    ghostSnap.SnapPoints = snapper.SnapPoints;
            //    ghostSnap.SnapLayer = snapper.SnapLayer;
            //}

            _ghostObject.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
            _ghostObject.transform.localScale = meshFilter.transform.lossyScale;
            if (_realObject.GetComponent<DemonPortal>() != null)
            {
                _ghostObject2.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
                _ghostObject2.transform.localScale = meshFilter.transform.lossyScale;
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

        public void Build()
        {
            if (_ghostObject2 != null)
            {
                if (_ghostObject2.GetComponent<Snapper>().IsColliding)
                {
                    _canBuild = false;
                }
            }
            
            DestroyGhostObject();
                if (_canBuild)
            {
                if (_realObject.GetComponent<DemonPortal>() != null)
                {
                    BuildPortal();
                }
                else if (_realObject.GetComponent<MachinePart>() != null)
                {
                    if (!BuildMachinePart()) return;
                }
                else if (_realObject.CompareTag(_outputTag))
                {
                    if (!BuildOutput()) return;
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

                    // Make new machine if an input is placed
                    if (go.CompareTag(_inputTag) && go.TryGetComponent(out BuildingFactoryBase building))
                    {
                        _machineManager.AttachToCurrentMachine(building);
                        _machineManager.AddMachine();
                    }

                    if (go.GetComponent<Snapper>() != null)
                    {
                        go.GetComponent<Snapper>().IsPlaced = true;
                    }
                }

                if (_econManager != null)
                {
                    _econManager.SubtractMoney(_currentCost);
                    _currentCost = 0;
                }


                _locked = false;
            }
            else
            {
                Debug.LogWarning("you can't build in impossible area");
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
                _portalManager.PlacePortal(_ghostObject.transform.localPosition, curr, next);
            }
        }

        private bool BuildMachinePart()
        {
            if (!_machineManager.CanBuildMachinePart()) return false;

            GameObject go = Instantiate(_realObject, _ghostObject.transform.position,
                _ghostObject.transform.rotation);

            //go.layer = _buildingLayer;

            if (go.TryGetComponent(out MachinePart machinePart))
            {
                _machineManager.AttachToCurrentMachine(machinePart);
            }

            if (go.GetComponent<Snapper>() != null)
            {
                go.GetComponent<Snapper>().IsPlaced = true;
            }

            return true;
        }

        private bool BuildOutput()
        {
            if (!_machineManager.CanBuildMachinePart()) return false;

            GameObject go = Instantiate(_realObject, _ghostObject.transform.position,
                _ghostObject.transform.rotation);

            //go.layer = _buildingLayer;

            if (go.TryGetComponent(out BuildingFactoryBase building))
            {
                _machineManager.AttachToCurrentMachine(building);
                _machineManager.FinishMachine();
            }
            return true;
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
            }
        }




    }
}



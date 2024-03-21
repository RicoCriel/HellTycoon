using System.Collections.Generic;
using Buildings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

        private string _buildTag;
        private GameObject _ghostObject;
        private GameObject _realObject;
        private bool _locked = false;
        private bool _isSnapped = false;
        private bool _canBuild = false;
        private int _currentCost = 0;

        // Rotation speed
        public float RotateSpeed = 100.0f;
        // Height change speed
        public float HeightChangeSpeed = 1.0f;


        private void Awake()
        {
            //BuildingPanelUI._onPartChosen += CreateGhostObject;
            BuildSideUI._onBuild += CreateGhostObject;
            Snapper.OnStartSnapping += LockObj;
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
        }

        private void MoveGhostObject(RaycastHit hit)
        {
            _ghostObject.transform.position = new Vector3(hit.point.x, hit.point.y /*+ GetObjectHeight(hit.transform)*/, hit.point.z);

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

            var meshFilter = _realObject.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = _realObject.GetComponentInChildren<MeshFilter>();
            }
            Snapper snapper = _realObject.GetComponent<Snapper>();
            if (snapper != null)
            {
                var ghostSnap = _ghostObject.AddComponent<Snapper>();
                ghostSnap.snapPoints = snapper.snapPoints;
                ghostSnap.snapLayer = snapper.snapLayer;
            }

            _ghostObject.GetComponent<MeshFilter>().sharedMesh = meshFilter.sharedMesh;
            _ghostObject.transform.localScale = meshFilter.transform.lossyScale;
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
                    if (_rootObject)
                        go.transform.SetParent(_rootObject.transform);

                    // Make new machine if an input is placed
                    if (go.CompareTag(_inputTag))
                    {
                        _machineManager.AttachToCurrentMachine(go);
                        _machineManager.AddMachine();
                    }

                    if (go.GetComponent<Snapper>() != null)
                    {
                        go.GetComponent<Snapper>()._isPlaced = true;
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
            DestroyGhostObject();
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

            // Finish machine when output is placed
            if (go.CompareTag(_outputTag))
            {
                _machineManager.AttachToCurrentMachine(go);
                _machineManager.FinishMachine(go);
            }
            else if (go.TryGetComponent(out MachinePart machinePart))
            {
                _machineManager.AttachToCurrentMachine(go);
                _machineManager.CurrentMachine.AddMachine(machinePart);
            }

            if (go.GetComponent<Snapper>() != null)
            {
                go.GetComponent<Snapper>()._isPlaced = true;
            }

            return true;
        }

        private bool BuildOutput()
        {
            if (!_machineManager.CanBuildMachinePart()) return false;

            GameObject go = Instantiate(_realObject, _ghostObject.transform.position,
                _ghostObject.transform.rotation);

            _machineManager.AttachToCurrentMachine(go);
            _machineManager.FinishMachine(go);

            return true;
        }

        public void CancelBuilding()
        {
            DestroyGhostObject();
            //ConstructionMode = false;
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
        }




    }
}

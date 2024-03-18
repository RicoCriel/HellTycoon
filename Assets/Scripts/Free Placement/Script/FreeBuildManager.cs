using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FreeBuild
{
    public class FreeBuildManager : MonoBehaviour
    {
        // Add Object to be built to list
        public List<GameObject> ConstructionItems = new List<GameObject>();

        // can select outline Color.
        public Color AbleAreaColor = new Color(0, 255, 0);
        public Color NotAbleAreaColor = new Color(255, 0, 0);
        public static bool ConstructionMode = false;

        [SerializeField] private Material _transParentMaterial;
        [SerializeField] private FreeBuildUI _uiManager;
        [SerializeField] private GameObject _rootObject;
        [SerializeField] private PortalManager _portalManager;
        [SerializeField] private LandLayerManager _landLayerManager;
        [SerializeField] private Transform _layer1;
        [SerializeField] private Transform _layer2;
        [SerializeField] private LayerMask _groundLayer;

        private string _buildTag;
        //
        private GameObject _ghostObject;
        private GameObject _realObject;
        private bool _locked = false;
        private bool _canBuild = false;

        // Movement speed
        public float MoveSpeed = 5.0f;
        // Rotation speed
        public float RotateSpeed = 100.0f;
        // Height change speed
        public float HeightChangeSpeed = 1.0f;


        private void Awake()
        {
            BuildingPanelUI._onPartChosen += CreateGhostObject;
        }

        public void CreateGhostObject(BuildingData data)
        {
            _realObject = data.Prefab;
            _buildTag = data.BuildTag;
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

            //
            if (isHit)
            {
                _uiManager.EnableUI();
                CreateGhostObject(hit);
                ConstructionMode = true;
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
            if (Input.GetKey(KeyCode.Q))
            {
                RotateGhostObject(RotateSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateGhostObject(-RotateSpeed * Time.deltaTime);
            }
        }

        private void MoveGhostObject(RaycastHit hit)
        {
            _ghostObject.transform.position = new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z);

            SetGhostOutLine(hit.transform.gameObject);
            //SetUiEvent(hit.transform.gameObject);
            _canBuild = hit.transform.gameObject.transform.gameObject.tag == _buildTag;
        }

        private void SetGhostOutLine(GameObject areaToBeBuilt)
        {
            Color color = _canBuild ? AbleAreaColor : NotAbleAreaColor;
            _ghostObject.GetComponent<FreeBuildObject>().SetObjectTransparent(color, _transParentMaterial);
        }

        private void CreateGhostObject(RaycastHit hit)
        {
            _ghostObject = Instantiate(_realObject, new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z), Quaternion.identity);

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
                else
                {
                    GameObject go = Instantiate(_realObject, _ghostObject.transform.position, _ghostObject.transform.rotation);
                    if (_rootObject)
                        go.transform.SetParent(_rootObject.transform);
                }

                _locked = false;
            }
            else
            {
                Debug.LogWarning("you can't build in impossible area");
            }
            DestroyGhostObject();
            ConstructionMode = false;
        }

        public void CancelBuilding()
        {
            DestroyGhostObject();
            ConstructionMode = false;
            _uiManager.DisableUI();
        }

        private void SetGhostOutline(GameObject areaToBeBuilt)
        {
            Color color = _canBuild ? AbleAreaColor : NotAbleAreaColor;
            _ghostObject.GetComponent<FreeBuildObject>().SetObjectTransparent(color, _transParentMaterial);
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

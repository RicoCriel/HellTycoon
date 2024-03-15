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
        
        public Material TransParentMaterial;
        public FreeBuildUI UiManager;
        public GameObject RootObject;
        public static bool ConstructionMode = false;
        [SerializeField] private PortalManager _portalManager;
        [SerializeField] private LandLayerManager _landLayerManager;
        [SerializeField] private Transform _layer1;
        [SerializeField] private Transform _layer2;
        //
        private GameObject _ghostObject;
        private GameObject _realObject;
        private bool _locked = false;

        // Movement speed
        public float MoveSpeed = 5.0f;
        // Rotation speed
        public float RotateSpeed = 100.0f;
        // Height change speed
        public float HeightChangeSpeed = 1.0f;


        //
        public void CreateGhostObject(string objName)
        {
            _realObject = ConstructionItems.Find(x => x.name == objName);
            //if (null == realObject)
            //{
            //    Debug.LogError("You have to list the objects you're trying to build on.");
            //    return;
            //}
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
                UiManager.OnUI();
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
                        bool isHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

                        if (isHit && ghostObject)
                        {
                            MoveGhostObject(hit);
                        }
                    }


                    //// Change height
                    //Vector3 newPosition = ghostObject.transform.position;
                    //newPosition.y += heightChange * Time.deltaTime * heightChangeSpeed;
                    //ghostObject.transform.position = newPosition;
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
            SetUIEvent(hit.transform.gameObject);
        }

        private void SetGhostOutLine(GameObject areaToBeBuilt)
        {
            Color color = CanBuild(areaToBeBuilt, _ghostObject) ? AbleAreaColor : NotAbleAreaColor;
            _ghostObject.GetComponent<FreeBuildObject>().SetObjectTransparent(color, TransParentMaterial);
        }
        // Other methods remain unchanged...


        private void CreateGhostObject(RaycastHit hit)
        {
            _ghostObject = Instantiate(_realObject, new Vector3(hit.point.x, hit.point.y + GetObjectHeight(hit.transform), hit.point.z), Quaternion.identity);

            SetGhostOutline(hit.transform.gameObject);
            SetUIEvent(hit.transform.gameObject);
        }

        private void RotateGhostObject(float angle)
        {
            if (_ghostObject)
            {
                _ghostObject.transform.Rotate(0, angle, 0);
            }
        }

        private void SetUIEvent(GameObject areaToBeBuilt)
        {
            UiManager.Build = delegate
            {
                if (CanBuild(areaToBeBuilt, _ghostObject))
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
                            portalManager.PlacePortal(ghostObject.transform.localPosition, curr, next);
                        }
                    }
                    else
                    {


                        GameObject go = Instantiate(_realObject, _ghostObject.transform.position, _ghostObject.transform.rotation);
                        if (RootObject)
                            go.transform.SetParent(RootObject.transform);
                    }

                    _locked = false;
                }
                else
                {
                    Debug.LogWarning("you can't build in impossible area");
                }
                DestroyGhostObject();
                ConstructionMode = false;
            };

            UiManager.Cancel = delegate
            {
                DestroyGhostObject();
                ConstructionMode = false;
            };
        }

        // Show Object to be built
        private void SetGhostOutline(GameObject areaToBeBuilt)
        {
            Color color = CanBuild(areaToBeBuilt, _ghostObject) ? AbleAreaColor : NotAbleAreaColor;
            _ghostObject.GetComponent<FreeBuildObject>().SetObjectTransparent(color, TransParentMaterial);
        }

        private bool CanBuild(GameObject areaToBeBuilt, GameObject go)
        {
            return (areaToBeBuilt.transform.gameObject.tag == go.GetComponent<FreeBuildObject>().ConstructionAreaTagName);
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
    } // class ConstructionManager
}

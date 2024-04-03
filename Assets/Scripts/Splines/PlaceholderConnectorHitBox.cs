using Buildings;
using Splines.Drawing;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
namespace Splines
{
    public class PlaceholderConnectorHitBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [FormerlySerializedAs("isOutput")]
        public bool IsSplineStart;

        public bool ImConnected = false;

        [SerializeField] private Transform _connectorPoint;

        [SerializeField] private Transform _connectorAnglePoint;


        [SerializeField] private MeshRenderer _myMeshRenderer;


        [SerializeField] private SplineDrawer _splineDrawer;


        public SplineView Spline;

        public BuildingBase myBuildingNode;

        //MyFactory

        private void Awake()
        {
            if (_myMeshRenderer == null)
                _myMeshRenderer = GetComponent<MeshRenderer>();


            if (IsSplineStart)
            {
                _myMeshRenderer.material.color = Color.green;
            }
            else
            {
                _myMeshRenderer.material.color = Color.red;
            }

            if (_splineDrawer == null)
                _splineDrawer = FindObjectOfType<SplineDrawer>();
        }

        private void OnDestroy()
        {
            if (Spline)
            {
                Destroy(Spline.gameObject);
            }
        }

        public Vector3 GetConnectorPointSpline()
        {
            return _connectorPoint.position;
            // return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorAnglePointSpline()
        {
            return _connectorAnglePoint.position;
            // return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorPointDirection()
        {
            return _connectorPoint.forward;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (!IsSplineStart && !ImConnected && _splineDrawer.HasStartedDrawing)
            {
                Debug.Log("Literally calling the stop at machine logic");
                _splineDrawer.StopDrawingSplineAtMachine(this, out SplineView splineConnection, false);
                Spline = splineConnection;
                ImConnected = true;
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            // throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // if (eventData.button == PointerEventData.InputButton.Left)
            // {
            //     if (!IsSplineStart)
            //     {
            //         Debug.Log("Literally calling the stop at machine logic");
            //         splineDrawer.StopDrawingSplineAtMachine(this);
            //     }
            //     // Add your mouse down logic here
            // }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (IsSplineStart && !ImConnected)
                {
                    _splineDrawer.StartDrawingSpline(this);
                    Debug.Log("StartDrawingSpline");
                }
                // Add your mouse down logic here
            }
        }



        public bool SpawnObject(GameObject gameObject)
        {
            if (_splineDrawer == null || Spline == null) return false;

            //_splineDrawer.SpawnSplineFollower(gameObject, Spline);
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            _splineDrawer.SpawnSplineFollower(gameObject, Spline, CallBack);


            return true;
        }
        private void CallBack(GameObject obj)
        {
            if (myBuildingNode.TryGetComponent(out MachinePart nextMachinePart))
            {
                if (obj.GetComponent<DemonFear>() != null)
                { 
                float fearlevel = obj.GetComponent<DemonFear>().FearLevel;
                if (fearlevel >= nextMachinePart.GetReqFearLevel())
                {
                    nextMachinePart.AddDemon(nextMachinePart._unprocessedDemonContainer, obj);
                }
                }
            }

            else if (myBuildingNode.TryGetComponent(out BuildingFactoryBase nextMachine))
            {
                Assert.IsFalse(obj == null);
                nextMachine.AddDemon(nextMachine._unprocessedDemonContainer, obj);
            }

            //if (myBuildingNode.TryGetComponent(out BuildingPortal NextPortal))
            //{
            //    //todo add opslorpcode Portal via buildingportal class.
            //}
        }


    }
}

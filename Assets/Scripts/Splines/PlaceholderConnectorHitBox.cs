using Buildings;
using Splines.Drawing;
using System;
using UnityEngine;
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

        public BuildingFactoryBase myBuildingNode;
        
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
        

        public Vector3 GetConnectorPointSpline()
        {
            Vector3 position = _connectorPoint.position;
            return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorAnglePointSpline()
        {
            Vector3 position = _connectorAnglePoint.position;
            return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorPointDirection()
        {
            return _connectorPoint.forward;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            if (!IsSplineStart && !ImConnected)
            {
                Debug.Log("Literally calling the stop at machine logic");
                _splineDrawer.StopDrawingSplineAtMachine(this, out SplineView splineConnection);
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

            _splineDrawer.SpawnSplineFollower(gameObject, Spline);
            
            // _splineDrawer.SpawnSplineFollower(gameObject, Spline, CallBack);
            
            
            return true;
        }
        private void CallBack(GameObject obj)
        {
            myBuildingNode.AddDemon(myBuildingNode._unprocessedDemonContainer, obj);
        }


    }
}

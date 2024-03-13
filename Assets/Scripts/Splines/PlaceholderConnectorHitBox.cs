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

        [SerializeField]
        private Transform ConnectorPoint;
        [SerializeField]
        private Transform ConnectorAnglePoint;

        [SerializeField]
        private MeshRenderer _myMeshRenderer;

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
        }

        public Vector3 GetConnectorPointSpline()
        {
            Vector3 position = ConnectorPoint.position;
            return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorAnglePointSpline()
        {
            Vector3 position = ConnectorAnglePoint.position;
            return new Vector3(position.x, 0, position.z);
        }

        public Vector3 GetConnectorPointDirection()
        {
            return ConnectorPoint.forward;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {

                if (!IsSplineStart)
                {
                    Debug.Log("StopDrawingSpline");
                }
                // Add your mouse down logic here
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (IsSplineStart)
                {
                    Debug.Log("StartDrawingSpline");
                }
                // Add your mouse down logic here
            }
        }
    }
}

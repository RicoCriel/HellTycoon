using System;
using UnityEngine;
namespace Splines
{
    public class PlaceholderConnectorHitBox : MonoBehaviour
    {
        public bool isOutput;

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


            if (isOutput)
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
    }
}

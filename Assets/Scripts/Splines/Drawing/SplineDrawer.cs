using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
namespace Splines.Drawing
{
    public class SplineDrawer : MonoBehaviour
    
    {
        public LayerMask groundLayer; // Ground layer to interact with
        public LayerMask SplineLayer; // Ground layer to interact with
        
        [Header("pointSize")]
        [Range(0.01f, 0.5f)]
        public float pointInterval = 0.2f; // Interval between captured points
        [Space]
        [SerializeField]
        [Range(0.2f, 5f)]
        private float maxpointDistanceOfSet = 0.5f;
        [SerializeField]
        [Space]
        [Range(0.1f, 1f)]
        private float minpointDistanceOfSet = 0.05f;

        [Header("SplineReferences")]
        [SerializeField]
        private SplineView _splineViewPrefab;
        
        [SerializeField]
        private Mesh _meshToUse;
        [SerializeField]
        private Material _materialToUse;
        
        
        [HideInInspector]
        public List<Vector3> points = new List<Vector3>();
        private float timer = 0f;
        private Vector3 lastPosition;
        private Vector3 mostRecentPoint;
        
        
        private bool hasStartedDrawing = false;
        
        [Header("SplineConnectors")]
        [SerializeField]
        private PlaceholderConnectorHitBox BoxIn;
        [SerializeField]
        private PlaceholderConnectorHitBox BoxOut;
        
        //fix later by having more uniform mesh prefabs...
        [Header("size")]
        [Range(1, 100)]
        [SerializeField]
        private float SizeTester;
        
        private SplineView instanciatedSpline;
        
        void Update()
        {
            if (hasStartedDrawing)
            {
                UpdateMeshWhileDrawing();
                
                CapturePoint();
                
                timer += Time.deltaTime;
                bool IsNewPointDueToTime = timer >= pointInterval;
                bool isTimeForNewPointDistanceWise = IsTimeForNewPointDistanceWise(mostRecentPoint);
                bool isNotYetTimeForNewPointMinDistanceWise = IsNotYetTimeForNewPointMinDistanceWise(mostRecentPoint);

                // Debug.Log("time: " + IsNewPointDueToTime + " maxdist: " + isNotYetTimeForNewPointMinDistanceWise + " lowdist: " + isTimeForNewPointDistanceWise);

                if ((IsNewPointDueToTime && isNotYetTimeForNewPointMinDistanceWise) || isTimeForNewPointDistanceWise)
                {
                    AddPointAndCallMethod(mostRecentPoint);
                    timer = 0f;
                }
            }
            
            if (Input.GetMouseButtonDown(0) && !hasStartedDrawing)
            {
                hasStartedDrawing = true;
                instanciatedSpline = Instantiate(_splineViewPrefab, transform.position, Quaternion.identity);
                
                //replace with the actual points
                points.Add(BoxIn.GetConnectorPointSpline());
                points.Add(BoxIn.GetConnectorAnglePointSpline());
                
                SplineMesh.Channel meshChannel = instanciatedSpline.AddMeshToGenerate(_meshToUse);
                float SplineSize = instanciatedSpline.GetSplineUniformSize();
                instanciatedSpline.SetMaterial(_materialToUse);
                instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 3);
                // instanciatedSpline.SetMeshSize(10);
                instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));

            }

            if (Input.GetMouseButtonUp(0) && hasStartedDrawing)
            {
                hasStartedDrawing = false;
            }
           
            
        }
        
        void AddPointAndCallMethod(Vector3 newPoint)
        {
            points.Add(newPoint);
 
            // Call your method here with the new point
            AddNewSplinePoint(newPoint);
        }

        void AddNewSplinePoint(Vector3 point)
        {
            instanciatedSpline.AddOnePoint(point + new Vector3(0, 0.1f, 0), 1, Vector3.zero);
        }
        
        void CapturePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                mostRecentPoint = hit.point;
                Collider[] colliders = Physics.OverlapSphere(mostRecentPoint, 2f, SplineLayer);
                if (colliders.Length > 0)
                {
                    Debug.LogWarning("Collision detected. Cannot add point.");
                    return;
                }
                
                // float intersectionheight = hit.point.y;
                // PenObject.MovePen(mostRecentPoint);

                instanciatedSpline.UpdateLastPoint(mostRecentPoint + new Vector3(0, 0.1f, 0), Vector3.zero);
            }
        }
        
        private void UpdateMeshWhileDrawing()
        {
            //updateMeshWhileDrawing
            SplineMesh.Channel meshChannel = instanciatedSpline.GetMeshChannel(0);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 2);
            instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));
        }
        
        private bool IsTimeForNewPointDistanceWise(Vector3 newPoint)
        {
            if (points.Count > 0)
            {
                Vector3 lastPoint = points[^1];
                float distance = Vector3.Distance(newPoint, lastPoint);
                if (distance > maxpointDistanceOfSet)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool IsNotYetTimeForNewPointMinDistanceWise(Vector3 newPoint)
        {
            if (points.Count > 0)
            {
                Vector3 lastPoint = points[^1];
                float distance = Vector3.Distance(newPoint, lastPoint);
                if (distance > minpointDistanceOfSet)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

    }
    
}

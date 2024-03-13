using Dreamteck.Splines;
using System;
using System.Collections;
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

        [Header("size")]
        [Range(0.1f, 20)]
        [SerializeField]
        private float SelfCollisionRange;

        private SplineView instanciatedSpline;




        void Update()
        {

            if (Input.GetMouseButtonDown(0) && hasStartedDrawing == false)
            {
                points.Clear();

                hasStartedDrawing = true;
                instanciatedSpline = Instantiate(_splineViewPrefab);

                //replace with the actual points
                points.Add(BoxIn.GetConnectorPointSpline());
                points.Add(BoxIn.GetConnectorAnglePointSpline());
                points.Add(BoxIn.GetConnectorAnglePointSpline());

                instanciatedSpline.AddPoints(points, 1, Vector3.zero);

                SplineMesh.Channel meshChannel = instanciatedSpline.AddMeshToGenerate(_meshToUse);
                float SplineSize = instanciatedSpline.GetSplineUniformSize();
                instanciatedSpline.SetMaterial(_materialToUse);
                instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 3);
                // instanciatedSpline.SetMeshSize(10);
                instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));
            }

            if (hasStartedDrawing)
            {
                // Debug.Log(points.Count);
                CapturePoint();

                timer += Time.deltaTime;
                bool IsNewPointDueToTime = timer >= pointInterval;
                bool isTimeForNewPointDistanceWise = IsTimeForNewPointDistanceWise(mostRecentPoint);
                bool isNotYetTimeForNewPointMinDistanceWise = IsNotYetTimeForNewPointMinDistanceWise(mostRecentPoint);

                // Debug.Log("time: " + IsNewPointDueToTime + " maxdist: " + isNotYetTimeForNewPointMinDistanceWise + " lowdist: " + isTimeForNewPointDistanceWise);

                if ((IsNewPointDueToTime && isNotYetTimeForNewPointMinDistanceWise) || isTimeForNewPointDistanceWise)
                {
                    if (PerformRayCast()) return;
                    if (collisioncheck()) return;

                    AddPointAndCallMethod(mostRecentPoint);
                    timer = 0f;
                }
                UpdateMeshWhileDrawing();
            }

            if (Input.GetMouseButtonUp(0) && hasStartedDrawing)
            {
                hasStartedDrawing = false;
                points.Add(BoxOut.GetConnectorAnglePointSpline());
                points.Add(BoxOut.GetConnectorPointSpline());
                
                instanciatedSpline.AddOnePoint(mostRecentPoint, 1 , Vector3.zero);
                instanciatedSpline.AddOnePoint(BoxOut.GetConnectorAnglePointSpline(), 1 , Vector3.zero);
                instanciatedSpline.AddOnePoint(BoxOut.GetConnectorPointSpline(), 1 , Vector3.zero);
                UpdateMeshWhileDrawing();
                
                StartCoroutine(TestsplineFollowers());
                // Destroy(instanciatedSpline.gameObject);
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
            instanciatedSpline.AddOnePoint(point, 1, Vector3.zero);
        }

        void CapturePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                mostRecentPoint = hit.point;
                if (PerformRayCast()) return;
                if (collisioncheck()) return;

                instanciatedSpline.UpdateLastPoint(mostRecentPoint, Vector3.zero);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(mostRecentPoint, SelfCollisionRange);

            if (points.Count > 2)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(mostRecentPoint, points[^1] + new Vector3(0, 0.5f, 0));
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

        private bool collisioncheck()
        {

            Collider[] colliders = Physics.OverlapSphere(mostRecentPoint, SelfCollisionRange, SplineLayer);

            if (colliders.Length > 0)
            {
                Debug.LogWarning("Collision detected. Cannot add point.");
                return true;
            }
            return false;
        }
        
        private bool PerformRayCast()
        {
            if (points.Count > 2)
            {
                Vector3 direction = (mostRecentPoint - points[^1]).normalized;
                float distance = (mostRecentPoint - points[^1]).magnitude;

                Ray ray = new Ray(points[^1], direction);
                RaycastHit[] hits = Physics.RaycastAll(ray, distance, SplineLayer);
                Debug.Log(hits.Length);
                if (hits.Length > 1)
                {
                    // Debug.Log("Hit detected on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                    return true;
                }
                else
                {
                    Debug.Log("No hit detected.");
                    return false;
                }

                // RaycastHit hit;
                // if (Physics.Raycast(points[^1], direction, out hit, distance, SplineLayer))
                // {
                //     Debug.Log("Hit detected on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                //     return true;
                // }
                // else
                // {
                //     Debug.Log("No hit detected.");
                //     return false;
                // }
            }
            return false;
        }
        
        
        [Header("SplineFollowerTest")]
        [SerializeField]
        private SplineFollowerView _followerViewPrefab;

        [SerializeField]
        [Range(0.1f, 3)]
        private float _timeBetweenSpawns = 1f;

        [SerializeField]
        [Range(1f, 100)]
        private float _spawnAmount = 1f;

        [SerializeField]
        [Range(1f, 20)]
        private float _followSpeed = 1f;
        
        IEnumerator TestsplineFollowers()
        {
            yield return new WaitForSeconds(1f);
            SplineComputer splineComputer = instanciatedSpline.GetSplinecomputer();

            for (int i = 0; i < _spawnAmount; i++)
            {
                Vector3 StartPoint = instanciatedSpline.GetSplineStartingPoint();
                SplineFollowerView follower = Instantiate(_followerViewPrefab, StartPoint, Quaternion.identity, instanciatedSpline.transform);
                follower.SetComputer(splineComputer);
                follower.SetFollow(true);
                follower.SetSpeed(_followSpeed);
                follower.SetFollowMode(SplineFollower.FollowMode.Uniform);

                // follower.HookUpEndReachedEvent();
                // follower.FollowerArrived += (sender, args) => {
                //     //Call machine code where the object just arrived.
                //     Destroy(args.GameObject);
                // };
                //

                EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
                followerArrivedHandler = (sender, args) => {
                    Debug.Log("Follower Arrived");
                    //Call machine code where the object just arrived.
                    follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival
                    Destroy(args.GameObject);
                };
                follower.FollowerArrived += followerArrivedHandler;

                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

    }

}

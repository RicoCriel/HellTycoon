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
        private bool CurrentSplineConnected = false;

        private PlaceholderConnectorHitBox currentStartingBox;
        private SplineView instanciatedSpline;

        //fix later by having more uniform mesh prefabs...
        [Header("size")]
        [Range(0.1f, 100)]
        [SerializeField]
        private float SizeTester;

        [Header("selfCollision")]
        [Header("size")]
        [Range(0.1f, 20)]
        [SerializeField]
        private float SelfCollisionRange;


        [Header("SplineFollowerTest")]
        [SerializeField]
        private SplineFollowerView _followerViewPrefab;

        [SerializeField]
        [Range(1f, 100)]
        private float _spawnAmount = 1f;

        [Header("BeltProperties")]
        [SerializeField]
        [Range(0.1f, 3)]
        private float _timeBetweenSpawns = 1f;
        [SerializeField]
        [Range(1f, 20)]
        private float _followSpeed = 1f;

        void Update()
        {
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
        }

        public void StartDrawingSpline(PlaceholderConnectorHitBox placeholderConnectorHitBox)
        {
            points.Clear();
            currentStartingBox = placeholderConnectorHitBox;

            CurrentSplineConnected = false;
            hasStartedDrawing = true;
            instanciatedSpline = Instantiate(_splineViewPrefab);

            //replace with the actual points
            points.Add(placeholderConnectorHitBox.GetConnectorPointSpline());
            points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            // points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());

            instanciatedSpline.AddPoints(points, 1, Vector3.zero);

            SplineMesh.Channel meshChannel = instanciatedSpline.AddMeshToGenerate(_meshToUse);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            instanciatedSpline.SetMaterial(_materialToUse);
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 3);
            // instanciatedSpline.SetMeshSize(10);
            instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));
        }

        public void StopDrawingSplineAtMachine(PlaceholderConnectorHitBox placeholderConnectorHitBox, out SplineView spline)
        {
            hasStartedDrawing = false;
            CurrentSplineConnected = true;
            // points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            points.Add(placeholderConnectorHitBox.GetConnectorPointSpline());

            // instanciatedSpline.AddOnePoint(mostRecentPoint, 1, Vector3.zero);
            // instanciatedSpline.AddOnePoint(placeholderConnectorHitBox.GetConnectorAnglePointSpline(), 1, Vector3.zero);
            instanciatedSpline.AddOnePoint(placeholderConnectorHitBox.GetConnectorPointSpline(), 1, Vector3.zero);
            UpdateMeshWhileDrawing();
            Debug.Log("Completing spline");

            // instanciatedSpline.SetSplineUpdateMode(SplineComputer.UpdateMode.None);
            spline = instanciatedSpline;

            OnSplineCompleted(new SplineConnectionCompletedEventArgs(instanciatedSpline, currentStartingBox, placeholderConnectorHitBox));
            currentStartingBox = null;
            instanciatedSpline = null;
        }

        //destroys spline for now when drawing is stopped and not at a relevant point
        private void LateUpdate()
        {
            if (Input.GetMouseButtonUp(0) && hasStartedDrawing && !CurrentSplineConnected)
            {
                // CurrentSplineConnected = false;
                currentStartingBox = null;
                hasStartedDrawing = false;
                Debug.Log("destroying spline");
                Destroy(instanciatedSpline.gameObject);
            }
        }


        private IEnumerator TestsplineFollowers()
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


                EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
                followerArrivedHandler = (sender, args) =>
                {
                    Debug.Log("Follower Arrived");
                    //Call machine code where the object just arrived.
                    follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival
                    Destroy(args.GameObject);
                };
                follower.FollowerArrived += followerArrivedHandler;

                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

        //add speed/ spawnrate parameters to this method if you want...
        public void SpawnSplineFollower(GameObject gameObject, SplineView computer /*, Machine arrivelMachine*/)
        {
            //get relevant data
            SplineComputer splineComputer = computer.GetSplinecomputer();
            Vector3 StartPoint = computer.GetSplineStartingPoint();

            //instantiate and parent demon to follower
            SplineFollowerView follower = Instantiate(_followerViewPrefab, StartPoint, Quaternion.identity, computer.transform);
            gameObject.transform.parent = follower.transform;

            //set up follower logic
            follower.SetComputer(splineComputer);
            follower.SetFollow(true);
            follower.SetSpeed(_followSpeed);
            follower.SetFollowMode(SplineFollower.FollowMode.Uniform);

            //hook up events
            EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
            followerArrivedHandler = (sender, args) =>
            {

                Debug.Log("Follower Arrived");
                //todo Call machine code where the object just arrived.

                follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival

                Destroy(args.GameObject);
            };
            follower.FollowerArrived += followerArrivedHandler;
        }


        private void AddPointAndCallMethod(Vector3 newPoint)
        {
            points.Add(newPoint);
            // mostRecentPoint = newPoint;


            // Call your method here with the new point
            AddNewSplinePoint(newPoint);
        }

        private void AddNewSplinePoint(Vector3 point)
        {
            instanciatedSpline.AddOnePoint(point, 1, Vector3.zero);

        }

        [SerializeField]
        public float selfColisionMultiplier = 2.1f;

        private void CapturePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {


                Vector3 hitPoint = hit.point;

                Vector3 directionToSecondPoint = (hitPoint - points[^1]).normalized;
                if (Vector3.Distance(hitPoint, points[^1]) < SelfCollisionRange * selfColisionMultiplier)
                {
                    mostRecentPoint = hitPoint;
                }
                else
                {
                    Vector3 newPoint = points[^1] + directionToSecondPoint * (SelfCollisionRange * selfColisionMultiplier);

                    mostRecentPoint = newPoint;
                }
                Debug.DrawLine(points[^1], points[^1] + Vector3.up * 3, Color.red, 0.0f);
                Debug.DrawLine(mostRecentPoint, mostRecentPoint + Vector3.up * 3, Color.green, 0.0f);
                Debug.DrawLine(hitPoint, hitPoint + Vector3.up * 3, Color.blue, 0.0f);

                if (PerformRayCast()) return;
                if (collisioncheck()) return;

                instanciatedSpline.UpdateLastPoint(mostRecentPoint, Vector3.zero);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(mostRecentPoint, SelfCollisionRange);

            // if (points.Count > 2)
            // {
            //     Gizmos.color = Color.red;
            //
            //     Gizmos.DrawLine(mostRecentPoint, points[^1] + new Vector3(0, 0.5f, 0));
            // }
        }


        private void UpdateMeshWhileDrawing()
        {
            //updateMeshWhileDrawing
            SplineMesh.Channel meshChannel = instanciatedSpline.GetMeshChannel(0);
            float SplineSize = instanciatedSpline.GetSplineUniformSize();
            instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)SplineSize * 2);
            instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(SizeTester, SizeTester, SizeTester));
            // instanciatedSpline.UpdateColliderInstantly();
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
            // if (points.Count > 2)
            // {
            //     Vector3 direction = (mostRecentPoint - points[^1]).normalized;
            //
            //     Vector3 newStartPoint = points[^1] + direction * 0.1f;
            //     float distance = (mostRecentPoint - newStartPoint).magnitude;
            //
            //     Debug.DrawLine(points[^1], points[^1] + Vector3.up *3 , Color.red, 0.0f);
            //     Debug.DrawLine(mostRecentPoint, mostRecentPoint + Vector3.up *3 , Color.green, 0.0f);
            //     Debug.DrawLine(newStartPoint, newStartPoint + Vector3.up *3 , Color.blue, 0.0f);
            //     
            //     Debug.DrawLine(newStartPoint, newStartPoint + direction * distance, Color.yellow, 0.0f);
            //
            //     RaycastHit hit;
            //
            //     if (Physics.Raycast(newStartPoint, direction, out hit, distance, SplineLayer))
            //     {
            //         Debug.Log("Hit detected on layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            //         return true;
            //     }
            //     else
            //     {
            //         Debug.Log("No hit detected.");
            //         return false;
            //     }
            // }
            return false;
        }


        //events

        public event EventHandler<SplineConnectionCompletedEventArgs> SplineCompleted;

        public void OnSplineCompleted(SplineConnectionCompletedEventArgs eventargs)
        {
            EventHandler<SplineConnectionCompletedEventArgs> handler = SplineCompleted;
            handler?.Invoke(this, eventargs);
        }
    }

    public class SplineConnectionCompletedEventArgs : EventArgs
    {
        public SplineView CurrentSpline;

        public PlaceholderConnectorHitBox ConnectorStart;
        //Machine start variable

        public PlaceholderConnectorHitBox ConnectorEnd;
        //Machine end variable

        public SplineConnectionCompletedEventArgs(SplineView currentSpline, PlaceholderConnectorHitBox connectorStart /*,Machine machineStart */, PlaceholderConnectorHitBox connectorEnd /*,Machine machineEnd */)
        {
            CurrentSpline = currentSpline;
            ConnectorStart = connectorStart;
            // Machinestart = machinestart;
            ConnectorEnd = connectorEnd;
            // Machineend = machineEnd;
        }

    }

}

using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using PopupSystem;
using Splines.Obstacles;
using Splines.Utillity;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
namespace Splines.Drawing
{
    public class SplineDrawer : MonoBehaviour

    {
        public LayerMask GroundLayer; // Ground layer to interact with

        public LayerMask SplineLayer; // Ground layer to interact with

        public LayerMask SupportBeamLayer; // Ground layer to interact with

        [Header("pointSize")]
        [Range(0.01f, 0.5f)]
        public float PointInterval = 0.2f; // Interval between captured points
        [Space]
        [Range(0f, 5f)]
        [SerializeField] private float _maxpointDistanceOfSet = 0.5f;
        [Space]
        [Range(0f, 1f)]
        [SerializeField] private float _minpointDistanceOfSet = 0.05f;
        [Space]
        [Header("General References")]
        // [SerializeField]
        // private bool UsingPathSpline;
        [SerializeField]
        private bool AddIndicationPath;
        [FormerlySerializedAs("_materialToUse")]
        [SerializeField] private Material _materialToUseMesh;
        [SerializeField] private Material _materialToUsePath;
        [Header("SplineReferences Mesh")]
        [SerializeField] private SplineView _splineViewPrefab;

        [SerializeField] private Mesh _meshToUse;

        // [Header("SplineReferences Path")]
        // [SerializeField]
        // private SplineView _splineViewPrefabPath;
        [SerializeField]
        [Range(0f, 0.5f)]
        private float _splinePathOffset;
        [SerializeField]
        [Range(0f, 0.1f)]
        private float _splinePathOffsetMinMAx;

        private float CurrentPointOffset;

        [HideInInspector]
        public List<Vector3> Points = new List<Vector3>();
        public List<Vector3> PointsWithHeight = new List<Vector3>();
        private float _timer = 0f;
        private Vector3 _lastPosition;
        private Vector3 _mostRecentPoint;


        private bool _hasStartedDrawing = false;
        public bool HasStartedDrawing => _hasStartedDrawing;

        private bool _currentSplineConnected = false;

        private PlaceholderConnectorHitBox _currentStartingBox;

        private SplineView _instanciatedSpline;


        //fix later by having more uniform mesh prefabs...
        [Header("size")]
        [Range(0.1f, 100)]
        [SerializeField] private float _sizeTester;

        [Header("selfCollision")]
        [Header("size")]
        [Range(0.1f, 20)]
        [SerializeField] private float _selfCollisionRange;
        public float SelfColisionMultiplier = 2.1f;


        [Header("SplineFollowerTest")]
        [SerializeField] private SplineFollowerView _followerViewPrefab;

        [Range(1f, 100)]
        [SerializeField] private float _spawnAmount = 1f;

        [Header("BeltProperties")]
        [Range(0.1f, 3)]
        [SerializeField] private float _timeBetweenSpawns = 1f;
        [Range(1f, 20)]
        [SerializeField] private float _followSpeed = 1f;

        [Header("PlacementVariables")]
        [Range(0, 100)]
        [SerializeField] private float _maxBeltLenght = 50;
        private float _currentMaxBeltLenght = 0;
        [SerializeField] private Color _BeltPlacementColor;
        [SerializeField] private Color _BeltPlacementColorMax;

        [FormerlySerializedAs("_mudLayer")]
        [FormerlySerializedAs("ObstacleLayer")]
        [Header("Mud")]
        [SerializeField]
        public LayerMask _splineObstacleLayer;
        private bool IsInObstacle = false;
        private SplineObstacle _currentSplineObstacle;

        // [Header("Energizer")]
        // public LayerMask ObstacleLayer; // Ground layer to interact with
        // [Range(1, 10)]
        // [SerializeField]
        // private float _mudCostMultiplier = 1;

        [FormerlySerializedAs("MAxSplineLayers")]
        [Header("Height")]
        public int MaxSplineLayers = 4;

        [Header("SplineSupportBeams")]
        [SerializeField] private GameObject _splineSupportBeamPrefab;
        [SerializeField] private float _splineSupportBeamHorizontalDistance = 1f;



        private void Awake()
        {

        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < MaxSplineLayers; i++)
            {

                Gizmos.color = Color.yellow;

                Gizmos.DrawWireSphere(_mostRecentPoint + new Vector3(0, _selfCollisionRange * (2 * i), 0), _selfCollisionRange);
            }


            // if (points.Count > 2)
            // {
            //     Gizmos.color = Color.red;
            //
            //     Gizmos.DrawLine(mostRecentPoint, points[^1] + new Vector3(0, 0.5f, 0));
            // }
        }


        void Update()
        {
            if (_hasStartedDrawing)
            {
                if (SetBeltColourDependingOnPlacementRange()) return;

                // Debug.Log(points.Count);
                CapturePoint();


                _timer += Time.deltaTime;
                bool isNewPointDueToTime = _timer >= PointInterval;
                bool isTimeForNewPointDistanceWise = IsTimeForNewPointDistanceWise(_mostRecentPoint);
                bool isNotYetTimeForNewPointMinDistanceWise = IsNotYetTimeForNewPointMinDistanceWise(_mostRecentPoint);

                // Debug.Log("time: " + IsNewPointDueToTime + " maxdist: " + isNotYetTimeForNewPointMinDistanceWise + " lowdist: " + isTimeForNewPointDistanceWise);

                if ((isNewPointDueToTime && isNotYetTimeForNewPointMinDistanceWise) || isTimeForNewPointDistanceWise)
                {
                    // if (PerformRayCast()) return;
                    // Debug.Log(ValidHeightFound(out int index) + "" + index);

                    // if (Collisioncheck()) return;
                    // if (MathBasedSelfCollisionCheck()) return;
                    // Debug.DrawLine(Points[^1], Points[^1] + Vector3.up * 3, Color.red, 0.0f);
                    // Debug.DrawLine(DistanceCheckPoint, DistanceCheckPoint + Vector3.up * 3, Color.green, 0.0f);
                    // Debug.DrawLine(hitPoint, hitPoint + Vector3.up * 3, Color.blue, 0.0f);

                    // Vector3 directionToSecondPoint = (_mostRecentPoint - Points[^1]).normalized;
                    // Vector3 DistanceCheckPoint = Points[^1] + directionToSecondPoint * (_selfCollisionRange * SelfColisionMultiplier);
                    // if (ExtendedDistanceCheck(_mostRecentPoint, DistanceCheckPoint)) return;

                    List<int> checkableLayers = DistanceBasedSelfCollisionCheck();
                    if (!ValidHeightFound(out int index, checkableLayers)) return;


                    AddPointAndCallMethod(_mostRecentPoint, index);

                    LowerBeltSizeWhenDrawingNewPoint();

                    _timer = 0f;
                }
                UpdateMeshWhileDrawing();
            }
        }
        private void LowerBeltSizeWhenDrawingNewPoint()
        {
            if (IsInObstacle)
            {
                Debug.Log(_instanciatedSpline.CalculateLenghtLastPoints());

                _currentMaxBeltLenght -= (_instanciatedSpline.CalculateLenghtLastPoints() * -(_currentSplineObstacle.ObstacleSpeedInfluence));
                // _currentMaxBeltLenght = Mathf.Clamp(_currentMaxBeltLenght, 0, _maxBeltLenght);
            }
        }
        private bool SetBeltColourDependingOnPlacementRange()
        {
            // ObstacleCheck();

            if (MaxsizeCheck())
            {
                if (AddIndicationPath)
                {
                    _instanciatedSpline.ChangeAllPointColours(_BeltPlacementColorMax);
                }
                return true;
            }

            if (AddIndicationPath)
            {
                float MaxBuildPercentage = _instanciatedSpline.GetSplineUniformSize() / _currentMaxBeltLenght;
                _instanciatedSpline.ChangePercentualPointColours(_BeltPlacementColor, MaxBuildPercentage);
            }
            return false;
        }
        private void ObstacleCheck()
        {

            Collider[] colliders = Physics.OverlapSphere(_mostRecentPoint, _selfCollisionRange, _splineObstacleLayer);

            if (colliders.Length > 0)
            {
                if (_currentSplineObstacle == null)
                {
                    IsInObstacle = true;
                    _currentSplineObstacle = colliders[0].GetComponent<SplineObstacle>();
                }
            }
            else
            {
                _currentSplineObstacle = null;
                IsInObstacle = false;
            }
        }
        private bool MaxsizeCheck()
        {
            float splineUniformSize = _instanciatedSpline.GetSplineUniformSize();

            return _currentMaxBeltLenght < splineUniformSize;
        }

        public SplineView StartDrawingSpline(PlaceholderConnectorHitBox placeholderConnectorHitBox)
        {
            PopupInstanceTracker.CurrentPopupInstance = null;
            _currentMaxBeltLenght = _maxBeltLenght;
            Points.Clear();

            PointsWithHeight.Clear();
            _currentStartingBox = placeholderConnectorHitBox;
            _currentSplineConnected = false;
            _hasStartedDrawing = true;
            _instanciatedSpline = Instantiate(_splineViewPrefab);

            //add start Box to spline
            _instanciatedSpline.StartConnector = placeholderConnectorHitBox;

            //replace with the actual points
            Points.Add(placeholderConnectorHitBox.GetConnectorPointSpline());
            // Points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            PointsWithHeight.Add(placeholderConnectorHitBox.GetConnectorPointSpline());
            // PointsWithHeight.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            // points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());

            _instanciatedSpline.AddPoints(Points, 1, Vector3.zero);

            // if (UsingPathSpline)
            // {
            SplineMesh.Channel meshChannel = _instanciatedSpline.AddMeshToGenerate(_meshToUse);
            float splineSize = _instanciatedSpline.GetSplineUniformSize();
            _instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)splineSize * 3);
            _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));

            _instanciatedSpline.SetMaterialMesh(_materialToUseMesh);

            if (AddIndicationPath)
                _instanciatedSpline.SetMaterialPath(_materialToUsePath);


            // instanciatedSpline.SetMeshSize(10);

            return _instanciatedSpline;
        }

        public float StopDrawingSplineAtMachine(PlaceholderConnectorHitBox placeholderConnectorHitBox, out SplineView spline)
        {
            if (!_hasStartedDrawing)
            {
                spline = null;
                return 0;
            }

            if (MaxsizeCheck())
            {
                spline = null;
                return 0;
            }

            _hasStartedDrawing = false;
            _currentSplineConnected = true;

            //add end box to spline

            _instanciatedSpline.setEndConnector(placeholderConnectorHitBox);
            _instanciatedSpline.setStartConnector(_currentStartingBox);
            _instanciatedSpline.DisablePathMeshRenderer();

            // points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            Points.Add(placeholderConnectorHitBox.GetConnectorPointSpline());
            PointsWithHeight.Add(placeholderConnectorHitBox.GetConnectorPointSpline());

            // instanciatedSpline.AddOnePoint(mostRecentPoint, 1, Vector3.zero);
            // instanciatedSpline.AddOnePoint(placeholderConnectorHitBox.GetConnectorAnglePointSpline(), 1, Vector3.zero);
            _instanciatedSpline.AddOnePoint(placeholderConnectorHitBox.GetConnectorPointSpline(), 1, Vector3.zero);
            UpdateMeshWhileDrawing();
            Debug.Log("Completing spline");

            // SetLayer(_instanciatedSpline.transform, SplineLayer);

            CreateSupportBeams();
            _instanciatedSpline.TurnOnMeshcollider();

            // instanciatedSpline.SetSplineUpdateMode(SplineComputer.UpdateMode.None);
            spline = _instanciatedSpline;

            var nextBuilding = placeholderConnectorHitBox.GetComponentInParent<BuildingFactoryBase>();
            if (nextBuilding != null)
            {
                _currentStartingBox.myBuildingNode = nextBuilding;
            }

            _currentStartingBox.ImConnected = true;

            _instanciatedSpline.SetPopupPositionToSplineMiddlePoint();
            // _instanciatedSpline.SetSplineUpdateMode(SplineComputer.UpdateMode.None);


            OnSplineCompleted(new SplineConnectionCompletedEventArgs(_instanciatedSpline, _currentStartingBox, placeholderConnectorHitBox));
            float splineSizetoReturn = _instanciatedSpline.GetSplineUniformSize();

            _currentStartingBox = null;
            _instanciatedSpline = null;

            return splineSizetoReturn;
        }

        public void CreateSupportBeams()
        {
            if (_instanciatedSpline == null) return;

            List<Vector3> points = _instanciatedSpline.GetSupportBeamPositions(0.2f);

            foreach (Vector3 point in points)
            {
                //raycast down to to check if any other objects are in the way
                if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, Mathf.Infinity, SupportBeamLayer))
                {
                    if (Vector3.Distance(hit.point, point) > 0.3f)
                    {
                        Instantiate(_splineSupportBeamPrefab, point, Quaternion.identity, _instanciatedSpline.transform);
                    }

                    // point.y = hit.point.y;
                }


            }
        }

        //destroys spline for now when drawing is stopped and not at a relevant point
        private void LateUpdate()
        {
            if (Input.GetMouseButtonUp(0) && _hasStartedDrawing && !_currentSplineConnected)
            {
                // CurrentSplineConnected = false;
                _currentStartingBox = null;
                _hasStartedDrawing = false;
                Debug.Log("destroying spline");
                Destroy(_instanciatedSpline.gameObject);
            }
        }

        private IEnumerator TestsplineFollowers()
        {
            yield return new WaitForSeconds(1f);
            SplineComputer splineComputer = _instanciatedSpline.GetSplinecomputer();

            for (int i = 0; i < _spawnAmount; ++i)
            {
                Vector3 startPoint = _instanciatedSpline.GetSplineStartingPoint();
                SplineFollowerView follower = Instantiate(_followerViewPrefab, startPoint, Quaternion.identity, _instanciatedSpline.transform);
                follower.SetComputer(splineComputer);
                follower.SetFollow(true);
                follower.SetSpeed(_followSpeed);
                follower.SetFollowMode(SplineFollower.FollowMode.Uniform);


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



        public void SpawnSplineFollower(GameObject gameObject, SplineView computer, Action<GameObject> callBack)
        {
            //get relevant data
            SplineComputer splineComputer = computer.GetSplinecomputer();
            Vector3 startPoint = computer.GetSplineStartingPoint();

            //instantiate and parent demon to follower
            SplineFollowerView follower = Instantiate(_followerViewPrefab, startPoint, Quaternion.identity, computer.transform);
            gameObject.transform.parent = follower.transform;
            gameObject.transform.localPosition = Vector3.zero;

            computer.AddSplineRider();

            //set up follower logic
            follower.SetComputer(splineComputer);
            follower.SetFollow(true);
            follower.SetSpeed(_followSpeed);
            follower.SetFollowMode(SplineFollower.FollowMode.Uniform);

            //hook up events
            EventHandler<FollowerArrivedEventArgs> followerArrivedHandler = null;
            followerArrivedHandler = (sender, args) => {
                if (args.GameObject == null) return;

                Debug.Log("Follower Arrived");
                //todo Call machine code where the object just arrived.

                follower.FollowerArrived -= followerArrivedHandler; // Unsubscribe after arrival

                computer.RemoveSplineRider();
                // Detach demon from spline follower
                DemonHandler demonHandler = args.GameObject.GetComponentInChildren<DemonHandler>();
                if (demonHandler != null)
                {
                    demonHandler.gameObject.transform.parent = null;

                    callBack(demonHandler.gameObject);
                }

                Destroy(args.GameObject);
            };
            follower.FollowerArrived += followerArrivedHandler;
        }


        private void AddPointAndCallMethod(Vector3 newPoint, int heightIndex)
        {
            Points.Add(newPoint);
            PointsWithHeight.Add(newPoint + new Vector3(0, CurrentPointOffset + (_selfCollisionRange * heightIndex * 2), 0));

            // if (heightIndex > 0)
            // {
            //     _instanciatedSpline.AddSupportBeamPosition((newPoint + new Vector3(0, CurrentPointOffset + (_selfCollisionRange * heightIndex * 2), 0)));
            // }

            // mostRecentPoint = newPoint;


            // Call your method here with the new point
            AddNewSplinePoint(newPoint, heightIndex);
        }

        private void AddNewSplinePoint(Vector3 point, int heightIndex)
        {
            float offset = GetNewYOffset();
            CurrentPointOffset = offset;

            _instanciatedSpline.AddOnePoint(point, 1, new Vector3(0, CurrentPointOffset + (_selfCollisionRange * heightIndex * 2), 0));

        }


        private void CapturePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayer))
            {
                Vector3 hitPoint = hit.point;

                Vector3 directionToSecondPoint = (hitPoint - Points[^1]).normalized;
                if (Vector3.Distance(hitPoint, Points[^1]) < _selfCollisionRange * SelfColisionMultiplier)
                {
                    _mostRecentPoint = hitPoint;
                }
                else
                {
                    Vector3 newPoint = Points[^1] + directionToSecondPoint * (_selfCollisionRange * SelfColisionMultiplier);

                    _mostRecentPoint = newPoint;
                }



                // Debug.DrawLine(Points[^1], Points[^1] + Vector3.up * 3, Color.red, 0.0f);
                // Debug.DrawLine(DistanceCheckPoint, DistanceCheckPoint + Vector3.up * 3, Color.green, 0.0f);
                // Debug.DrawLine(hitPoint, hitPoint + Vector3.up * 3, Color.blue, 0.0f);

                // Vector3 DistanceCheckPoint = Points[^1] + directionToSecondPoint * (_selfCollisionRange * SelfColisionMultiplier);
                // if (ExtendedDistanceCheck(hitPoint, DistanceCheckPoint)) return;

                // if(MathBasedSelfCollisionCheck()) return;
                List<int> checkableLayers = DistanceBasedSelfCollisionCheck();
                if (!ValidHeightFound(out int index, checkableLayers)) return;
                //
                // if (!ValidHeightFound(out int index))
                // {
                //     return;
                // }
                // if (DistanceBasedCollisionCheck(index))
                // {
                //     if (!ValidHeightFound(out int index2, index))
                //     {
                //             
                //     }
                //     else
                //     {
                //         return;
                //     }
                // }



                _instanciatedSpline.UpdateLastPoint(_mostRecentPoint, new Vector3(0, CurrentPointOffset + (_selfCollisionRange * index * 2), 0), Color.white);
            }
        }
        private bool ExtendedDistanceCheck(Vector3 hitPoint, Vector3 DistanceCheckPoint)
        {

            if (Vector3.Distance(hitPoint, DistanceCheckPoint) > 0.01f)
            {
                if (MathBasedSelfCollisionCheck(hitPoint, DistanceCheckPoint))
                {
                    Debug.Log("Collision detected. Cannot add point.");
                    return true;
                }
                Debug.Log("no collision detected add point.");
            }
            else
            {
                Debug.Log("distance not big enough.");
                return true;
            }
            return false;
        }

        private float GetNewYOffset()
        {
            float offset = UnityEngine.Random.Range(-_splinePathOffsetMinMAx, _splinePathOffsetMinMAx);

            float result = _splinePathOffset + offset;

            return result;
        }


        private void UpdateMeshWhileDrawing()
        {
            //updateMeshWhileDrawing

            SplineMesh.Channel meshChannel = _instanciatedSpline.GetMeshChannel(0);
            float splineSize = _instanciatedSpline.GetSplineUniformSize();
            _instanciatedSpline.SetMeshGenerationCount(meshChannel, (int)splineSize * 3);
            _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));


            // instanciatedSpline.UpdateColliderInstantly();
        }

        private bool IsTimeForNewPointDistanceWise(Vector3 newPoint)
        {
            if (Points.Count > 0)
            {
                Vector3 lastPoint = Points[^1];
                float distance = Vector3.Distance(newPoint, lastPoint);
                if (distance > _maxpointDistanceOfSet)
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
            if (Points.Count > 0)
            {
                Vector3 lastPoint = Points[^1];
                float distance = Vector3.Distance(newPoint, lastPoint);
                if (distance > _minpointDistanceOfSet)
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

        private bool MathBasedSelfCollisionCheck()
        {
            return MathBasedIntersection.LineIntersectsItself3D(PointsWithHeight, out int pointIndex, true);
        }

        private bool MathBasedSelfCollisionCheck(Vector3 MostRecentPoint, Vector3 PredictionPoint)
        {
            return MathBasedIntersection.LineIntersectsItself3D(PointsWithHeight, MostRecentPoint, PredictionPoint, out int pointIndex, true);
        }

        private bool Collisioncheck()
        {
            Collider[] colliders = Physics.OverlapSphere(_mostRecentPoint, _selfCollisionRange, SplineLayer);

            if (colliders.Length > 0)
            {
                Debug.LogWarning("Collision detected. Cannot add point.");
                return true;
            }
            return false;
        }

        private List<int> DistanceBasedSelfCollisionCheck()
        {
            int endIndex = PointsWithHeight.Count - 3;

            List<int> selfcollisionLayers = new List<int>();

            for (int i = 0; i < MaxSplineLayers; i++)
            {
                Vector3 MRPWithHeight = _mostRecentPoint + new Vector3(0, _selfCollisionRange * (2 * i), 0);

                for (int y = 0; y < endIndex; y++)
                {
                    Vector3 point = PointsWithHeight[y];
                    float distance = Vector3.Distance(MRPWithHeight, point);
                    if (distance < _selfCollisionRange * SelfColisionMultiplier)
                    {
                        Debug.DrawLine(MRPWithHeight, point, Color.red, 0f);
                        selfcollisionLayers.Add(i);
                        break;
                    }
                }
            }
            return selfcollisionLayers;
        }

        private bool ValidHeightFound(out int index, List<int> CantCheck = null)
        {
            for (int i = 0; i < MaxSplineLayers; i++)
            {
                if (CantCheck != null && CantCheck.Contains(i))
                {
                    continue;
                }
                Collider[] colliders = Physics.OverlapSphere(_mostRecentPoint + new Vector3(0, _selfCollisionRange * (2 * i), 0), _selfCollisionRange, SplineLayer);

                if (colliders.Length > 0) continue;

                index = i;
                return true;
                // break;
            }
            index = 5;
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

        public void OnSplineCompleted(SplineConnectionCompletedEventArgs e)
        {
            e.ConnectorStart.Spline = e.CurrentSpline;

            EventHandler<SplineConnectionCompletedEventArgs> handler = SplineCompleted;
            handler?.Invoke(this, e);
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

public enum SplineType
{
    MeshOnly,
    PathOnly,
    MeshAndPath
}

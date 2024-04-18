using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using PopupSystem;
using Splines.Data;
using Splines.Drawing;
using Splines.Obstacles;
using Splines.SplineMEsh;
using Splines.Utillity;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
namespace Splines.Drawing
{
    public class SplineDrawer : MonoBehaviour

    {
        [Header("splineBuildingMode")]
        public bool SplineBuildingModeActive = false;
        [SerializeField]
        private bool PlaceConnectorWhenMouseUp = true;

        [Header("LayerMasks")]
        public LayerMask GroundLayer; // Ground layer to interact with

        [FormerlySerializedAs("SplineLayer")]
        public LayerMask Buildings; // Ground layer to interact with
        public LayerMask BuildingsAndConnectors; // Ground layer to interact with

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
        [SerializeField]
        private bool AddIndicationPath;
        [FormerlySerializedAs("_materialToUse")]
        [SerializeField] private Material _materialToUsePath;
        [FormerlySerializedAs("_splineViewPrefab")]
        [Header("SplineReferences Mesh")]
        [SerializeField] private SplineView _ConstructingSplinePrefab;
        [SerializeField] private SplineViewMesh MeshLayerPrefab;

        [SerializeField]
        private bool CreateInstantMesh = false;
        [FormerlySerializedAs("_materialToUseConstructing")]
        [FormerlySerializedAs("_meshToUseElevated")]
        [FormerlySerializedAs("_meshToUse")]
        [SerializeField] private Material _materialToUseMesh;
        [FormerlySerializedAs("_meshToUseConstructing")]
        [SerializeField] private Mesh _meshToUse;

        [Space]
        [Header("Mesh Layer Data Settings")]
        [SerializeField]
        private MeshDataList _meshDataList;
        private Dictionary<SplineType, SplineMeshPart> MeshLayers = new Dictionary<SplineType, SplineMeshPart>();

        // [Header("SplineReferences Path")]
        [SerializeField]
        [Range(0f, 0.5f)]
        private float _splinePathOffset;
        [SerializeField]
        [Range(0f, 0.1f)]
        private float _splinePathOffsetMinMAx;
        private float CurrentPointOffset;


        //fix later by having more uniform mesh prefabs...
        [Header("size")]
        [Range(0.1f, 100)]
        [SerializeField] private float _sizeTester;

        [Range(0.2f, 10)]
        [SerializeField]
        private float MeshSampleRateModifyer = 3;

        [Header("selfCollision")]
        [Header("size")]
        [Range(0.1f, 20)]
        [SerializeField] private float _selfCollisionRange;
        public float SelfColisionMultiplier = 2.1f;


        [Header("SplineFollowerTest")]
        [SerializeField] private SplineFollowerView _followerViewPrefab;

        [Header("PlacementVariables")]
        [Range(0, 500)]
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

        [FormerlySerializedAs("MAxSplineLayers")]
        [Header("Height")]
        public int MaxSplineLayers = 4;

        [Header("SplineSupportBeams")]
        [SerializeField] private GameObject _splineSupportBeamPrefab;
        [SerializeField] private float _splineSupportBeamHorizontalDistance = 1f;

        //bools and private references
        private bool _hasStartedDrawing = false;
        public bool HasStartedDrawing => _hasStartedDrawing;
        private bool _currentSplineConnected = false;
        private PlaceholderConnectorHitBox _currentStartingBox;
        private SplineView _instanciatedSpline;




        //point stuff
        [HideInInspector]
        // public List<Vector3> Points = new List<Vector3>();
        // public List<Vector3> PointsWithHeight = new List<Vector3>();
        private float _timer = 0f;
        // private Vector3 _lastPosition;
        // private Vector3 _mostRecentPoint;

        public readonly SplinePointModelList SplinePointModels = new SplinePointModelList();
        private SplinePointModel _mostRecentPointModel;
        private Camera _camera;

        [Header("connector")]
        [SerializeField] private BuildingFactoryExtender _extenderPrefab;

        private void Awake()
        {
            _camera = Camera.main;

        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < MaxSplineLayers; i++)
            {

                Gizmos.color = Color.yellow;
                if (_mostRecentPointModel != null)
                {

                    Gizmos.DrawWireSphere(_mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * i), 0), _selfCollisionRange);
                }

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

                //update last point
                CapturePoint();


                _timer += Time.deltaTime;
                bool isNewPointDueToTime = _timer >= PointInterval;
                bool isTimeForNewPointDistanceWise = IsTimeForNewPointDistanceWise(_mostRecentPointModel.WorldPositionGround);
                bool isNotYetTimeForNewPointMinDistanceWise = IsNotYetTimeForNewPointMinDistanceWise(_mostRecentPointModel.WorldPositionGround);

                if ((isNewPointDueToTime && isNotYetTimeForNewPointMinDistanceWise) || isTimeForNewPointDistanceWise)
                {
                    List<int> checkableLayers = DistanceBasedSelfCollisionCheck();
                    if (!ValidHeightFound(out int index, checkableLayers, HeightPref)) return;

                    AddNewPoint(_mostRecentPointModel, index);

                    HeightPref = index;

                    LowerBeltSizeWhenDrawingNewPoint();

                    _timer = 0f;
                }
                UpdateMeshWhileDrawing();
            }
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(1) && _hasStartedDrawing && !_currentSplineConnected)
            {
                if (PlaceConnectorWhenMouseUp)
                {
                    //find last splinePoints
                    SplinePointModel lastPoint = SplinePointModels.GetLastSplinePointModel();
                    SplinePointModel firstToLastPoint = SplinePointModels.GetFirsttoLastSplinePointModel();

                    if (lastPoint.Height == 0)
                    {
                        Debug.Log("Creating extender");
                        //calculate Flat Rotation
                        Vector3 direction = lastPoint.WorldPosition - firstToLastPoint.WorldPosition;
                        direction.Normalize();
                        Vector3 newPoint = lastPoint.WorldPosition + direction * 2f;
                        Quaternion rotation = Quaternion.LookRotation(direction);

                        float yRotation = rotation.eulerAngles.y;
                        // Quaternion yQuaternion = Quaternion.Euler(0, yRotation, 0);
                        Quaternion finalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);

                        Vector3 cumulativeSize = GetPrefabCumulativeSize(_extenderPrefab.gameObject, finalRotation);
                        // DrawBounds(newPoint, cumulativeSize);
                        if (!CheckExtenderCollision(newPoint, cumulativeSize, finalRotation))
                        {
                            if (!CheckForDistanceBasedSelfCollisionAtPoint(newPoint))
                            {
                                BuildingFactoryExtender spawnedExtender = Instantiate(_extenderPrefab, newPoint, finalRotation);

                                StopDrawingSplineAtMachine(spawnedExtender._entryBoxes[0], out SplineView foundSpline, true);
                                spawnedExtender._entryBoxes[0].Spline = foundSpline;
                                spawnedExtender._entryBoxes[0].ImConnected = true;
                            }
                            else
                            {
                                MessagePopupHUD.TriggerPopup("No Space For Extender Here due to SelfCollision");
                                // Debug.Log("No Space For Extender Here due to SelfCollision");
                            }
                        }
                        MessagePopupHUD.TriggerPopup("No Space For Extender Here");
                        // Debug.Log("No Space For Extender Here");
                    }
                    MessagePopupHUD.TriggerPopup("Conveyor belt currently not on ground");
                } // CurrentSplineConnected = false;
            }
            else if (Input.GetMouseButtonUp(0) && _hasStartedDrawing && !_currentSplineConnected)
            {
                MessagePopupHUD.TriggerPopup("Deleting Conveyor Belt");
                DeleteSpline();
            }
        }

        // void DrawBounds(Vector3 center, Vector3 size)
        // {
        //     center.y += size.y / 2;
        //     Vector3 halfSize = size * 0.5f;
        //
        //     // Calculate the corners of the bounds
        //     Vector3[] corners = new Vector3[8];
        //     corners[0] = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        //     corners[1] = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        //     corners[2] = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        //     corners[3] = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        //     corners[4] = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        //     corners[5] = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        //     corners[6] = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
        //     corners[7] = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        //
        //     // Draw lines between the corners to visualize the bounds
        //     Debug.DrawLine(corners[0], corners[1], Color.red, 50f);
        //     Debug.DrawLine(corners[1], corners[2], Color.red, 50f);
        //     Debug.DrawLine(corners[2], corners[3], Color.red, 50f);
        //     Debug.DrawLine(corners[3], corners[0], Color.red, 50f);
        //
        //     Debug.DrawLine(corners[4], corners[5], Color.red, 50f);
        //     Debug.DrawLine(corners[5], corners[6], Color.red, 50f);
        //     Debug.DrawLine(corners[6], corners[7], Color.red, 50f);
        //     Debug.DrawLine(corners[7], corners[4], Color.red, 50f);
        //
        //     Debug.DrawLine(corners[0], corners[4], Color.red, 50f);
        //     Debug.DrawLine(corners[1], corners[5], Color.red, 50f);
        //     Debug.DrawLine(corners[2], corners[6], Color.red, 50f);
        //     Debug.DrawLine(corners[3], corners[7], Color.red, 50f);
        // }
        Vector3 GetPrefabCumulativeSize(GameObject prefab, Quaternion rotation)
        {
            // Get all renderers in the prefab and its children
            Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

            // Initialize the cumulative bounds
            Bounds cumulativeBounds = new Bounds(prefab.transform.position, Vector3.zero);

            // Combine the bounds of all renderers
            foreach (Renderer renderer in renderers)
            {
                // Get the bounds of the renderer
                Bounds rendererBounds = renderer.bounds;

                // Transform the bounds by the rotation quaternion
                // Bounds transformedBounds = RotateBounds(rendererBounds, rotation, prefab.transform.position);

                // Encapsulate the transformed bounds
                cumulativeBounds.Encapsulate(rendererBounds);
            }

            // Return the size of the cumulative bounds
            return cumulativeBounds.size;
        }

        Bounds RotateBounds(Bounds bounds, Quaternion rotation, Vector3 center)
        {
            Vector3 extents = bounds.extents;
            Vector3 rotatedExtents = rotation * extents;
            return new Bounds(center, rotatedExtents * 2);
        }

        bool CheckExtenderCollision(Vector3 Position, Vector3 size, quaternion rotation)
        {
            // Perform a collision check using an overlap box
            var numColliders = Physics.OverlapBox(Position + new Vector3(0, size.y / 2, 0), size / 2, rotation, BuildingsAndConnectors);

            // Check if any colliders were found
            return numColliders.Length > 0;
        }

        private void DeleteSpline()
        {
            _currentStartingBox = null;
            _hasStartedDrawing = false;
            Debug.Log("destroying spline");
            Destroy(_instanciatedSpline.gameObject);
        }

        private float HeightBalanceTimer = 0;
        private float OffSetHeightBalanceTimer = 0.5f;


        private int _heightPref = 0;
        public int HeightPref{
            get {
                return _heightPref;
            }
            set {
                if (_heightPref != value)
                {
                    HeightBalanceTimer = 0;
                }
                _heightPref = value;
            }
        }

        private void SplineHeightBalancer()
        {
            HeightBalanceTimer += Time.deltaTime;

            if (HeightBalanceTimer > OffSetHeightBalanceTimer)
            {
                if (HeightPref == MaxSplineLayers)
                {
                    HeightPref = MaxSplineLayers - 1;
                }
                else
                {
                    HeightPref -= 1;
                }
                if (HeightPref < 0)
                {
                    HeightPref = 0;
                }
            }
        }


        //public methods
        public SplineView StartDrawingSpline(PlaceholderConnectorHitBox placeholderConnectorHitBox)
        {
            if (!SplineBuildingModeActive) return null;

            PopupInstanceTracker.CurrentPopupInstance = null;
            Vector3 connectorPointSpline = placeholderConnectorHitBox.GetConnectorPointSpline();

            if (CanFindPointHeightOffset(connectorPointSpline, out int foundheight, out Vector3 foundGroundPoint))
            {
                _currentMaxBeltLenght = _maxBeltLenght;
                SplinePointModels.ClearSplinePointModels();

                // PointsWithHeight.Clear();
                _currentStartingBox = placeholderConnectorHitBox;
                _currentSplineConnected = false;
                _hasStartedDrawing = true;
                _instanciatedSpline = Instantiate(_ConstructingSplinePrefab);

                //add start Box to spline
                _instanciatedSpline.StartConnector = placeholderConnectorHitBox;
                HeightBalanceTimer = 0;
                _heightPref = 0;
                //replace with the actual points


                //find connector in and out
                _heightPref = foundheight;
                // SplinePointModels.AddSplinePointModel(
                //     new SplinePointModel(foundGroundPoint,
                //         connectorPointSpline,
                //         foundheight, 1, Color.white, SplinePointModels.GetSplinePointModelCount()));
                //
                // _instanciatedSpline.AddOnePoint(SplinePointModels.GetLastSplinePointModel().WorldPosition, 1, Vector3.zero);
                // _mostRecentPointModel = SplinePointModels.GetLastSplinePointModel();

                SplinePointModels.AddSplinePointModel(
                    new SplinePointModel(foundGroundPoint,
                        connectorPointSpline,
                        foundheight, 1, Color.white, SplinePointModels.GetSplinePointModelCount()));

                _instanciatedSpline.AddOnePoint(SplinePointModels.GetLastSplinePointModel().WorldPosition, 1, Vector3.zero);
                _mostRecentPointModel = SplinePointModels.GetLastSplinePointModel();

                if (CreateInstantMesh)
                {
                    SplineMesh.Channel meshChannel = _instanciatedSpline.AddMeshToGenerate(_meshToUse);
                    float splineSize = _instanciatedSpline.GetSplineUniformSize();
                    _instanciatedSpline.SetMeshGenerationCount(meshChannel, Mathf.RoundToInt(splineSize * MeshSampleRateModifyer));
                    _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));

                    _instanciatedSpline.SetMaterialMesh(_materialToUseMesh);

                }
                if (AddIndicationPath)
                    _instanciatedSpline.SetMaterialPath(_materialToUsePath);
            }

            // instanciatedSpline.SetMeshSize(10);

            return _instanciatedSpline;
        }
        private bool CanFindPointHeightOffset(Vector3 connectorPointSpline, out int foundHeight, out Vector3 hitpoint)
        {
            Ray ray = new Ray(connectorPointSpline, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayer))
            {
                Vector3 hitPoint = hit.point;

                float distance = Vector3.Distance(connectorPointSpline, hitPoint);

                float layerResult = distance / (_selfCollisionRange * 2);
                foundHeight = Mathf.FloorToInt(layerResult);
                hitpoint = hitPoint;
                return true;
            }
            foundHeight = MaxSplineLayers + 1;
            hitpoint = Vector3.zero;
            return false;
        }

        public float StopDrawingSplineAtMachine(PlaceholderConnectorHitBox placeholderConnectorHitBox, out SplineView spline, bool isMAxSizeSplineAllowed)
        {
            if (!_hasStartedDrawing)
            {
                spline = null;
                return 0;
            }

            if (MaxsizeCheck() && !isMAxSizeSplineAllowed)
            {
                spline = _instanciatedSpline;
                return 0;
            }

            Vector3 connectorPointSpline = placeholderConnectorHitBox.GetConnectorPointSpline();
            if (!CanFindPointHeightOffset(connectorPointSpline, out int foundheight, out Vector3 foundGroundPoint))
            {
                spline = null;
                return 0;
            }

            // _instanciatedSpline.VisualizeSplineTangents();



            _hasStartedDrawing = false;
            _currentSplineConnected = true;

            //add end box to spline

            _instanciatedSpline.setEndConnector(placeholderConnectorHitBox);
            _instanciatedSpline.setStartConnector(_currentStartingBox);
            _instanciatedSpline.DisablePathMeshRenderer();

            // points.Add(placeholderConnectorHitBox.GetConnectorAnglePointSpline());
            SplinePointModels.AddSplinePointModel(
                new SplinePointModel(connectorPointSpline,
                    connectorPointSpline,
                    0, 1, Color.white, SplinePointModels.GetSplinePointModelCount()));

            _instanciatedSpline.AddOnePoint(SplinePointModels.GetLastSplinePointModel().WorldPositionGround, 1, Vector3.zero);


            // List<(SplineType, List<SplinePointModel>)> meshDivisions = SplinePointModels.findMeshDivisions();
            // ConstructPartualMeshes(meshDivisions);

            // UpdateMeshWhileDrawing();

            CreateCumulativeSplineView();

            SplinePointModels.SetsplinepointForwards();
            List<Vector3> result = SplinePointModels.CreateSupportBeamPositions(0.2f);
            SplinePointModels.RemoveRandomPointsSupportBeams(0.6f, result);
            CreateSupportBeams(result);
            Debug.Log("Completing spline");

            // SetLayer(_instanciatedSpline.transform, SplineLayer);


            _instanciatedSpline.TurnOnMeshcollider();

            // instanciatedSpline.SetSplineUpdateMode(SplineComputer.UpdateMode.None);
            spline = _instanciatedSpline;

            BuildingFactoryBase nextBuilding = placeholderConnectorHitBox.GetComponentInParent<BuildingFactoryBase>();
            if (nextBuilding != null)
            {
                _currentStartingBox.myBuildingNode = nextBuilding;
                placeholderConnectorHitBox.myBuildingNode = nextBuilding;
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
        private void CreateCumulativeSplineView()
        {
            SplineView splineView = _instanciatedSpline;

            SplinePointModelList splinePointModels = SplinePointModels.cloneSplinePointModelList();
            splinePointModels.SetsplinepointForwards();

            int usedMeshChunkVertAmount = _meshDataList.MeshDataParts[0].Mesh.vertexCount;
            float splineSize = splineView.GetSplineUniformSize();
            int meshSampleRate = Mathf.RoundToInt(splineSize * MeshSampleRateModifyer);

            int splinepointamount = splinePointModels.GetSplinePointModelCount();
            int AmountOfVerticies = meshSampleRate * usedMeshChunkVertAmount;
            int MinAmountOfSplineChuncks = Mathf.CeilToInt((float)AmountOfVerticies / 65000);
            int AmountOfSplineChuncks = Mathf.Max(MinAmountOfSplineChuncks, 1);
            int amountofVertPerPoint = AmountOfVerticies / splinePointModels.GetSplinePointModelCount();

            Debug.Log("splinepointamount: " + splinepointamount + " AmountOfVerticies: " + AmountOfVerticies + " MinAmountOfSplineChuncks: " + MinAmountOfSplineChuncks + " AmountOfSplineChuncks: " + AmountOfSplineChuncks + " amountofVertPerPoint: " + amountofVertPerPoint);

            int vertexThreshold = 30000; // Change this value as needed

            // Track current vertex count
            int currentVertexCount = 0;
            float similarityThreshold = 0.9f; // Adjust as needed

            // Determine chunk boundaries based on forward vector similarity
            List<List<SplinePointModel>> splineChunks = new List<List<SplinePointModel>>();
            List<SplinePointModel> currentChunk = new List<SplinePointModel>();
            currentChunk.Add(splinePointModels.GetSplinePointModel(0));
            for (int i = 1; i < splinepointamount; i++)
            {
                // Check forward vector similarity
                float forwardSimilarity = Vector3.Dot(splinePointModels.GetSplinePointModel(i - 1).SplinePointForward, splinePointModels.GetSplinePointModel(i).SplinePointForward);

                // Add vertex count for current spline point
                currentVertexCount += amountofVertPerPoint;

                // If similarity drops below threshold or vertex count exceeds threshold,
                // start a new chunk
                if (forwardSimilarity < similarityThreshold && currentVertexCount > vertexThreshold)
                {
                    splineChunks.Add(currentChunk);
                    currentChunk = new List<SplinePointModel>();
                    currentChunk.Add(splinePointModels.GetSplinePointModel(i - 1));
                    currentChunk.Add(splinePointModels.GetSplinePointModel(i));
                    currentVertexCount = 0; // Reset vertex count for new chunk
                }
                else
                {
                    currentChunk.Add(splinePointModels.GetSplinePointModel(i));
                }
            }

            // Add the last chunk
            splineChunks.Add(currentChunk);

            foreach (List<SplinePointModel> SplineChunk in splineChunks)
            {
                SplineViewMesh splineviewMesh = Instantiate(MeshLayerPrefab, splineView.transform);
                splineView.AddSplineMeshPart(splineviewMesh);
                splineviewMesh.AddPoints(SplineChunk);
                splineviewMesh.RebuildComputerImmidiate();
                splineviewMesh.SetMaterialMesh(_materialToUseMesh);

                SplineMesh.Channel channel = splineviewMesh.AddMeshToGenerate(_meshDataList.MeshDataParts[0].Mesh);
                splineviewMesh.RandomizeSeed(channel);
                for (int index = 1; index < _meshDataList.MeshDataParts.Count; index++)
                {
                    MeshDataPart meshData = _meshDataList.MeshDataParts[index];
                    splineviewMesh.AddMeshToChannel(channel, meshData.Mesh);
                }
                splineviewMesh.SetChannelRandomOrder(channel, true);
                float splineSizeChunk = splineviewMesh.GetSplineUniformSize();
                int meshSampleRateChunk = Mathf.RoundToInt(splineSizeChunk * MeshSampleRateModifyer);
                splineviewMesh.SetMeshGenerationCount(channel, meshSampleRateChunk);
                splineviewMesh.SetMeshSCale(channel, new Vector3(_sizeTester * 0.9f, _sizeTester * 0.9f, _sizeTester * 0.9f),
                    new Vector3(_sizeTester * 1.1f, _sizeTester * 1.1f, _sizeTester * 1.1f));
                splineviewMesh.SetChannelYRandomRotation(channel, 12);
            }
        }
        private void ConstructPartualMeshes(List<(SplineType, List<SplinePointModel>)> meshDivisions)

            // SplineMesh.Channel meshChannel = _instanciatedSpline.GetMeshChannel(0);
            // float splineSize = _instanciatedSpline.GetSplineUniformSize();
            // _instanciatedSpline.SetMeshGenerationCount(meshChannel, Mathf.RoundToInt(splineSize * MeshSampleRateModifyer));
            // _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));
        {

        }

        public void SpawnSplineFollower(GameObject gameObject, SplineView computer, Action<GameObject, GameObject> callBack)
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
            follower.SetSpeed(4);
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
                    GameObject grandParent = demonHandler.gameObject.transform.parent.parent.gameObject;
                    demonHandler.gameObject.transform.parent = null;

                    callBack(demonHandler.gameObject, grandParent);
                }

                Destroy(args.GameObject);
            };
            follower.FollowerArrived += followerArrivedHandler;
        }

        public void CreateSupportBeams(List<Vector3> positions)
        {
            if (_instanciatedSpline == null) return;


            foreach (Vector3 point in positions)
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

        //point adding / updating
        private void AddNewPoint(SplinePointModel newPoint, int heightIndex)
        {
            SplinePointModels.AddSplinePointModel(newPoint.Clone());
            // Points.Add(newPoint);
            // PointsWithHeight.Add(newPoint + new Vector3(0, CurrentPointOffset + (_selfCollisionRange * heightIndex * 2), 0));

            float offset = GetNewYOffset();
            CurrentPointOffset = offset;

            _instanciatedSpline.AddOnePoint(newPoint.WorldPosition, 1, Vector3.zero);
            // _instanciatedSpline.AddOnePoint(newPoint, 1, new Vector3(0, CurrentPointOffset + (_selfCollisionRange * heightIndex * 2), 0));
        }
        private void CapturePoint()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            // Ray newray = new Ray(_mostRecentPointModel.WorldPosition, Vector3.down * 100);
            // _mostRecentPointModel.WorldPosition

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayer))
            {
                float camdistance = Vector3.Distance(_camera.transform.position, hit.point);

                Vector3 normalizedYDirectionCameraPoint = (hit.point - _camera.transform.position);
                normalizedYDirectionCameraPoint.y = 0;


                Vector3 hitPoint = hit.point;
                // if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                // {
                //     
                // }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    hitPoint.y += 0.3f;
                }

                SelfCollisionDistanceCheck(hitPoint);

                SplineHeightBalancer();
                List<int> checkableLayers = DistanceBasedSelfCollisionCheck();
                if (!ValidHeightFound(out int index, checkableLayers, HeightPref)) return;

                //
                // Vector3 point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camdistance*0.7f));
                // point.y = _mostRecentPointModel.WorldPositionGround.y + SelfCollisionRange(index);
                // Debug.DrawRay(point, normalizedYDirectionCameraPoint.normalized * 10, Color.blue);
                // Debug.DrawRay(hit.point, Vector3.up * 10, Color.green);


                _mostRecentPointModel.Height = index;
                _mostRecentPointModel.WorldPosition = _mostRecentPointModel.WorldPositionGround + new Vector3(0, SelfCollisionRange(index), 0);

                _instanciatedSpline.UpdateLastPoint(_mostRecentPointModel, Color.white);

            }
        }
        private float SelfCollisionRange(int index)
        {

            return CurrentPointOffset + (_selfCollisionRange * index * 2);
        }
        private void SelfCollisionDistanceCheck(Vector3 hitPoint)
        {

            Vector3 worldPositionGround = SplinePointModels.GetLastSplinePointModel().WorldPositionGround;
            if (Vector3.Distance(hitPoint, worldPositionGround) < _selfCollisionRange * SelfColisionMultiplier)
            {
                _mostRecentPointModel = new SplinePointModel(hitPoint, 1, SplinePointModels.GetSplinePointModelCount());
            }
            else
            {
                Vector3 directionToSecondPoint = (hitPoint - worldPositionGround).normalized;
                Vector3 newPoint = worldPositionGround + directionToSecondPoint * (_selfCollisionRange * SelfColisionMultiplier);

                _mostRecentPointModel = new SplinePointModel(newPoint, 1, SplinePointModels.GetSplinePointModelCount());
            }
        }

        private void UpdateMeshWhileDrawing()
        {
            if (CreateInstantMesh)
            {
                SplineMesh.Channel meshChannel = _instanciatedSpline.GetMeshChannel(0);
                float splineSize = _instanciatedSpline.GetSplineUniformSize();
                _instanciatedSpline.SetMeshGenerationCount(meshChannel, Mathf.RoundToInt(splineSize * MeshSampleRateModifyer));
                _instanciatedSpline.SetMeshSCale(meshChannel, new Vector3(_sizeTester, _sizeTester, _sizeTester));
            }
        }

        //spline drawing logic / new point calling
        private bool IsTimeForNewPointDistanceWise(Vector3 newPoint)
        {
            if (SplinePointModels.GetSplinePointModelCount() > 0)
            {
                Vector3 lastPoint = SplinePointModels.GetLastSplinePointModel().WorldPositionGround;
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
            if (SplinePointModels.GetSplinePointModelCount() > 0)
            {
                Vector3 lastPoint = SplinePointModels.GetLastSplinePointModel().WorldPositionGround;
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

        //collision checking
        private List<int> DistanceBasedSelfCollisionCheck()
        {
            int endIndex = SplinePointModels.GetSplinePointModelCount() - 3;

            List<int> selfcollisionLayers = new List<int>();

            for (int i = 0; i < MaxSplineLayers; i++)
            {
                Vector3 MRPWithHeight = _mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * i), 0);

                for (int y = 0; y < endIndex; y++)
                {
                    Vector3 point = SplinePointModels.GetSplinePointModel(y).WorldPosition;
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

        private bool CheckForDistanceBasedSelfCollisionAtPoint(Vector3 checkPoint)
        {
            int endIndex = SplinePointModels.GetSplinePointModelCount() - 3;

            for (int y = 0; y < endIndex; y++)
            {
                Vector3 point = SplinePointModels.GetSplinePointModel(y).WorldPosition;
                float distance = Vector3.Distance(checkPoint, point);
                if (distance < _selfCollisionRange * SelfColisionMultiplier)
                {
                    Debug.DrawLine(checkPoint, point, Color.red, 0f);
                    return true;
                }
            }
            return false;
        }
        // private bool ValidHeightFound(out int index, List<int> CantCheck = null, int preferredHeight = 0)
        // {
        //     // Check preferred height first
        //     if (preferredHeight >= 0 && preferredHeight < MaxSplineLayers)
        //     {
        //         // If the preferred height is not in the list of heights to avoid, and it's clear, return it
        //         Vector3 worldPositionGround = _mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * preferredHeight), 0);
        //         if ((CantCheck == null || !CantCheck.Contains(preferredHeight)) &&
        //             Physics.OverlapSphere(worldPositionGround, _selfCollisionRange, Buildings).Length == 0)
        //         {
        //             index = preferredHeight;
        //             return true;
        //         }
        //     }
        //
        //     // Search other heights
        //     for (int i = 0; i < MaxSplineLayers; i++)
        //     {
        //         if (i == preferredHeight || (CantCheck != null && CantCheck.Contains(i)))
        //         {
        //             continue; // Skip the preferred height and any heights in the CantCheck list
        //         }
        //
        //         Vector3 worldPositionGround = _mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * i), 0);
        //         Collider[] colliders = Physics.OverlapSphere(worldPositionGround, _selfCollisionRange, Buildings);
        //
        //         if (colliders.Length == 0)
        //         {
        //             index = i;
        //             return true;
        //         }
        //     }
        //
        //     index = 5; // If no valid height found, set a default index (you might want to handle this differently)
        //     return false;
        // }

        private bool ValidHeightFound(out int index, List<int> CantCheck = null, int preferredHeight = 0)
        {
            // Buffer for storing colliders found by OverlapSphereNonAlloc
            Collider[] buffer = new Collider[MaxSplineLayers];

            // Check preferred height first
            if (preferredHeight >= 0 && preferredHeight < MaxSplineLayers)
            {
                // If the preferred height is not in the list of heights to avoid, and it's clear, return it
                Vector3 worldPositionGround = _mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * preferredHeight), 0);
                int count = Physics.OverlapSphereNonAlloc(worldPositionGround, _selfCollisionRange, buffer, Buildings);

                if ((CantCheck == null || !CantCheck.Contains(preferredHeight)) && count == 0)
                {
                    index = preferredHeight;
                    return true;
                }
            }

            // Search other heights
            for (int i = 0; i < MaxSplineLayers; i++)
            {
                if (i == preferredHeight || (CantCheck != null && CantCheck.Contains(i)))
                {
                    continue; // Skip the preferred height and any heights in the CantCheck list
                }

                Vector3 worldPositionGround = _mostRecentPointModel.WorldPositionGround + new Vector3(0, _selfCollisionRange * (2 * i), 0);
                int count = Physics.OverlapSphereNonAlloc(worldPositionGround, _selfCollisionRange, buffer, Buildings);

                if (count == 0)
                {
                    index = i;
                    return true;
                }
            }

            index = 5; // If no valid height found, set a default index (you might want to handle this differently)
            return false;
        }


        //helper methods
        private float GetNewYOffset()
        {
            float offset = UnityEngine.Random.Range(-_splinePathOffsetMinMAx, _splinePathOffsetMinMAx);

            float result = _splinePathOffset + offset;

            return result;
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

        private bool MaxsizeCheck()
        {
            float splineUniformSize = _instanciatedSpline.GetSplineUniformSize();

            return _currentMaxBeltLenght < splineUniformSize;
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

// public enum SplineType
// {
//     MeshOnly,
//     PathOnly,
//     MeshAndPath
// }

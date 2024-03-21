using Dreamteck.Splines;
using PopupSystem;
using PopupSystem.Inheritors;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using PopupClickedEventArgs = PopupSystem.PopupClickedEventArgs;
namespace Splines
{
    public class SplineView : MonoBehaviour
    {
        [SerializeField] private bool IsPathSpline;

        [SerializeField] private SplineComputer _mySplineComputer;

        [SerializeField] private SplineMesh _mySplineMesh;

        [SerializeField] private MeshRenderer _myMeshRenderer;

        [SerializeField] private MeshFilter _myMeshFilter;

        [SerializeField] private PathGenerator _splinePath;

        public PlaceholderConnectorHitBox StartConnector;
        public PlaceholderConnectorHitBox EndConnector;


        [FormerlySerializedAs("_popupFactory")]
        [Header("popup")]
        [SerializeField]
        private WorldSpacePopupSpline _popupSpline;
        [SerializeField]
        private BuildingPopupActivator _popupActivator;

        public int splineRiders = 0;
        
        public void AddSplineRider()
        {
            splineRiders++;
            if (_popupSpline.IsPopupActive())
            {
                _popupSpline.SetSoulsOnSplineCounter(splineRiders);
            }
        }
        
        public void RemoveSplineRider()
        {
            splineRiders--;
            if (_popupSpline.IsPopupActive())
            {
                _popupSpline.SetSoulsOnSplineCounter(splineRiders);
            }
        }
        
        
        //popup stuff

        private void Awake()
        {
            if (_mySplineComputer == null)
                _mySplineComputer = GetComponent<SplineComputer>();

            if (_mySplineMesh == null)
                _mySplineMesh = GetComponent<SplineMesh>();

            if (_myMeshRenderer == null)
                _myMeshRenderer = GetComponent<MeshRenderer>();

            if (_myMeshFilter == null)
                _myMeshFilter = GetComponent<MeshFilter>();
            
            _popupActivator.SetPopupper(_popupSpline);
            _popupSpline.DestroyButtonClicked += OnDestroyButtonClicked;
        }

        private void OnDisable()
        {
            _popupSpline.DestroyButtonClicked -= OnDestroyButtonClicked;
       
        }
        private void OnDestroyButtonClicked(object sender, PopupClickedEventArgs e)
        {
            //todo UnParent followers and let them run around/try to escape, now they just disappear

            StartConnector.ImConnected = false;
            EndConnector.ImConnected = false;
            Destroy(gameObject);
        }

        public void SetPopupPositionToSplineMiddlePoint()
        {
            int middlePointIndex = _mySplineComputer.GetPoints().Length / 2;
            Vector3 middlePoint = _mySplineComputer.GetPointPosition(middlePointIndex);
            middlePoint.y += 1;

            _popupSpline.OverRideLocalPosition(middlePoint);
        }

        public void setStartConnector(PlaceholderConnectorHitBox startConnector)
        {
            StartConnector = startConnector;
        }

        public void setEndConnector(PlaceholderConnectorHitBox endConnector)
        {
            EndConnector = endConnector;
        }
        public SplineComputer GetSplinecomputer()
        {
            return _mySplineComputer;
        }

        public Vector3 GetSplineStartingPoint()
        {
            SplinePoint[] splinePoints = _mySplineComputer.GetPoints();
            return splinePoints.Length > 0 ? splinePoints[0].position : Vector3.zero;
        }

     

        public float GetSplineUniformSize()
        {
            return _mySplineComputer.CalculateLength();
        }

        /// <summary>
        /// Set the orientation of one of the spline Points Manually, this could be usefull when placing we want a manual orientation at the star and end for machines
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="splinePointIndex"></param>
        public void SetPointOrientation(Vector3 orientation, int splinePointIndex)
        {
            if (_mySplineComputer.GetPoints().Length <= splinePointIndex) return;
            SplinePoint[] points = _mySplineComputer.GetPoints();
            points[splinePointIndex].normal = orientation;

        }

        /// <summary>
        /// Add a Series op points to the spline
        /// </summary>
        /// <param name="points"></param>
        /// <param name="pointSize"></param>
        /// <param name="offset"></param>
        public void AddPoints(List<Vector3> points, float pointSize, Vector3 offset)
        {
            SplinePoint[] newPoints = new SplinePoint[points.Count];

            for (int i = 0; i < newPoints.Length; i++)
            {
                newPoints[i] = new SplinePoint();
                newPoints[i].position = points[i] + offset;
                newPoints[i].normal = Vector3.up;
                newPoints[i].size = pointSize;
                newPoints[i].color = Color.white;
            }

            _mySplineComputer.SetPoints(newPoints);
        }

        /// <summary>
        /// Update the splines last point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="offset"></param>
        public void UpdateLastPoint(Vector3 point, Vector3 offset, Color color)
        {
            if (_mySplineComputer.GetPoints().Length > 0)
            {
                SplinePoint[] points = _mySplineComputer.GetPoints();
                points[points.Length - 1].position = point + offset;
                // points[points.Length - 1].size = PointSize;
                points[points.Length - 1].normal = Vector3.up;
                points[points.Length - 1].color = color;
                _mySplineComputer.SetPoints(points);
            }
        }


        /// <summary>
        /// Add one point to the spline at the end
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pointSize"></param>
        /// <param name="offset"></param>
        public void AddOnePoint(Vector3 point, float pointSize, Vector3 offset)
        {
            SplinePoint[] points = _mySplineComputer.GetPoints();
            SplinePoint[] newPoints = new SplinePoint[points.Length + 1];
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = points[i];
            }
            newPoints[points.Length] = new SplinePoint();
            newPoints[points.Length].position = point + offset;
            newPoints[points.Length].size = pointSize;
            newPoints[points.Length].normal = Vector3.up;
            newPoints[points.Length].color = Color.white;
            _mySplineComputer.SetPoints(newPoints);
        }

        public void ChangeAllPointColours(Color color)
        {
            SplinePoint[] points = _mySplineComputer.GetPoints();
            for (int index = 0; index < points.Length; index++)
            {
                points[index].color = color;

            }
            _mySplineComputer.SetPoints(points);
        }

        public void ChangePercentualPointColours(Color color, float percent)
        {
            if (percent > 1)
            {
                percent = 1;
            }

            SplinePoint[] points = _mySplineComputer.GetPoints();

            int pointsToColour = (int)(points.Length * percent);

            for (int index = 0; index < pointsToColour; index++)
            {
                points[index].color = color;
            }
            _mySplineComputer.SetPoints(points);
        }

        /// <summary>
        /// remove point at any instance
        /// </summary>
        /// <param name="pointToRemove"></param>
        public void RemovePoint(int pointToRemove)
        {
            SplinePoint[] points = _mySplineComputer.GetPoints();
            SplinePoint[] newPoints = new SplinePoint[points.Length - 1];
            for (int i = 0; i < points.Length; i++)
            {
                if (i < pointToRemove) newPoints[i] = points[i];
                else if (i > pointToRemove) newPoints[i - 1] = points[i];
            }
            _mySplineComputer.SetPoints(newPoints);

        }

        /// <summary>
        /// Set spline update mode, Should be set to none when building is done to save performance
        /// </summary>
        /// <param name="mode"></param>
        public void SetSplineUpdateMode(SplineComputer.UpdateMode mode)
        {
            _mySplineComputer.updateMode = mode;
        }

        /// <summary>
        /// toggle MEshRenderer
        /// </summary>
        public void ToggleMeshRenderer()
        {
            _myMeshRenderer.enabled = !_myMeshRenderer.enabled;
        }

        /// <summary>
        /// Set the spline mesh single material
        /// </summary>
        /// <param name="material"></param>
        public void SetMaterial(Material material)
        {
            _myMeshRenderer.material = material;
        }

        /// <summary>
        /// set spline type between linear, bezier, hermite, Catmul rom etc.
        /// </summary>
        /// <param name="type"></param>
        public void SetSplineType(Spline.Type type)
        {
            _mySplineComputer.type = type;
        }

        /// <summary>
        /// Set sline samplemode, should be uniform for mesh generation
        /// </summary>
        /// <param name="sampleMode"></param>
        public void SetSampleMode(SplineComputer.SampleMode sampleMode)
        {
            _mySplineComputer.sampleMode = sampleMode;
        }

        /// <summary>
        /// Add a splinefollower to the spline, move them manually though.
        /// </summary>
        /// <param name="ObjectThatFollows"></param>
        /// <returns></returns>
        public void SetMeshUpdateMode(SplineMesh.UpdateMethod mode)
        {
            _mySplineMesh.updateMethod = mode;
        }

        public SplineMesh.Channel AddMeshToGenerate(Mesh mesh)
        {
            return _mySplineMesh.AddChannel(mesh, "Main");
        }
        public SplineMesh.Channel GetMeshChannel(int index)
        {
            return _mySplineMesh.GetChannel(index);
        }

        public void SetMeshGenerationCount(SplineMesh.Channel channel, int count)
        {
            channel.count = count;
        }

        public void SetMeshSCale(SplineMesh.Channel channel, Vector3 scale)
        {
            channel.maxScale = scale;
            channel.minScale = scale;
        }

        public void SetMeshSize(float size)
        {
            _mySplineMesh.size = size;
        }

        public void UpdateColliderInstantly()
        {
            _mySplineMesh.UpdateCollider();
        }

    }
}

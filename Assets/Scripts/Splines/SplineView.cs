using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
namespace Splines
{
    public class SplineView : MonoBehaviour
    {
        [SerializeField]
        private SplineComputer _mySplineComputer;
        [SerializeField]
        private SplineMesh _mySplineMesh;
        [SerializeField]
        private MeshRenderer _myMeshRenderer;
        [SerializeField]
        private MeshFilter _myMeshFilter;

        public SplineComputer GetSplinecomputer()
        {
            return _mySplineComputer;
        }

        public Vector3 GetSplineStartingPoint()
        {
            SplinePoint[] splinePoints = _mySplineComputer.GetPoints();
            return splinePoints.Length>0 ? splinePoints[0].position : Vector3.zero;
        }
        
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
        }
        
        public float GetSplineUniformSize()
        {
            return _mySplineComputer.CalculateLength();
        }

        /// <summary>
        /// Set the orientation of one of the spline Points Manually, this could be usefull when placing we want a manual orientation at the star and end for machines
        /// </summary>
        /// <param name="orientation"></param>
        /// <param name="SplinePointIndex"></param>
        public void SetPointOrientation(Vector3 orientation, int SplinePointIndex)
        {
            if (_mySplineComputer.GetPoints().Length <= SplinePointIndex) return;
            SplinePoint[] points = _mySplineComputer.GetPoints();
            points[SplinePointIndex].normal = orientation;
            
        }

        /// <summary>
        /// Add a Series op points to the spline
        /// </summary>
        /// <param name="Points"></param>
        /// <param name="PointSize"></param>
        /// <param name="Offset"></param>
        public void AddPoints(List<Vector3> Points, float PointSize, Vector3 Offset)
        {
            SplinePoint[] points = new SplinePoint[Points.Count];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new SplinePoint();
                points[i].position = Points[i] + Offset;
                points[i].normal = Vector3.up;
                points[i].size = PointSize;
                points[i].color = Color.white;
            }

            _mySplineComputer.SetPoints(points);
        }

        /// <summary>
        /// Update the splines last point
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Offset"></param>
        public void UpdateLastPoint(Vector3 Point, Vector3 Offset)
        {
            if (_mySplineComputer.GetPoints().Length > 0)
            {
                SplinePoint[] points = _mySplineComputer.GetPoints();
                points[points.Length - 1].position = Point + Offset;
                // points[points.Length - 1].size = PointSize;
                points[points.Length - 1].normal = Vector3.up;
                points[points.Length - 1].color = Color.white;
                _mySplineComputer.SetPoints(points);
            }
        }


        /// <summary>
        /// Add one point to the spline at the end
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="PointSize"></param>
        /// <param name="Offset"></param>
        public void AddOnePoint(Vector3 Point, float PointSize, Vector3 Offset)
        {
            SplinePoint[] points = _mySplineComputer.GetPoints();
            SplinePoint[] newPoints = new SplinePoint[points.Length + 1];
            for (int i = 0; i < points.Length; i++)
            {
                newPoints[i] = points[i];
            }
            newPoints[points.Length] = new SplinePoint();
            newPoints[points.Length].position = Point + Offset;
            newPoints[points.Length].size = PointSize;
            newPoints[points.Length].normal = Vector3.up;
            newPoints[points.Length].color = Color.white;
            _mySplineComputer.SetPoints(newPoints);
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

    }
}

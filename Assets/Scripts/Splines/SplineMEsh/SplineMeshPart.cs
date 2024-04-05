using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
namespace Splines.SplineMEsh
{
    public class SplineMeshPart : MonoBehaviour
    {

        [SerializeField] private SplineComputer _mySplineComputer;

        [SerializeField] private SplineMesh _mySplineMesh;

        [SerializeField] private MeshRenderer _splineMeshMeshRenderer;

        [SerializeField] private MeshFilter _splineMeshMeshFilter;

        private void Awake()
        {
            if (_mySplineComputer == null)
                _mySplineComputer = GetComponent<SplineComputer>();

            if (_mySplineMesh == null)
                _mySplineMesh = GetComponent<SplineMesh>();

            if (_splineMeshMeshRenderer == null)
                _splineMeshMeshRenderer = GetComponent<MeshRenderer>();

            if (_splineMeshMeshFilter == null)
                _splineMeshMeshFilter = GetComponent<MeshFilter>();
        }
        
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

        public void SetMaterialMesh(Material material)
        {
            _splineMeshMeshRenderer.material = material;
        }

        public void SetSampleMode(SplineComputer.SampleMode sampleMode)
        {
            _mySplineComputer.sampleMode = sampleMode;
        }
        public void SetMeshUpdateMode(SplineMesh.UpdateMethod mode)
        {
            _mySplineMesh.updateMethod = mode;
        }

        public SplineMesh.Channel AddMeshToGenerate(Mesh mesh)
        {
            return _mySplineMesh.AddChannel(mesh, "Main");
        }

        public void RemoveMeshChannel(int index)
        {
            _mySplineMesh.RemoveChannel(index);
        }

        public SplineMesh.Channel AddMeshToGenerate(string channel, Mesh mesh)
        {
            return _mySplineMesh.AddChannel(mesh, channel);
        }

        public SplineMesh.Channel GetMeshChannel(int index)
        {
            return _mySplineMesh.GetChannel(index);
        }

        public double GetPercentualPoint(int startIndex)
        {
            SplinePoint[] points = _mySplineComputer.GetPoints();

            double Percentage = _mySplineComputer.GetPointPercent(startIndex);

            return Percentage;
        }

        public void SetMeshGenerationCount(SplineMesh.Channel channel, int count)
        {
            channel.count = count;
        }

        public void SetMeshGenerationLocationPercentual(SplineMesh.Channel channel, double startPercent, double endPercent)
        {
            channel.clipFrom = startPercent;
            channel.clipTo = endPercent;

        }

        public void SetMeshSCale(SplineMesh.Channel channel, Vector3 scale)
        {
            channel.maxScale = scale;
            channel.minScale = scale;
        }

    }
}

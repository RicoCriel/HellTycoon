using Splines.Drawing;
using UnityEngine;
namespace Splines.Data
{
    [System.Serializable]
    public class MeshDataPart
    {
        public SplineType MeshType;

        public Mesh Mesh;
        public Material Material;
        [Range(0.1f, 50)]
        public float Size;
    }
}

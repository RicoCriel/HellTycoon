using UnityEngine;
namespace Splines.Drawing
{
    public class SplinePointModel
    {
        public Vector3 WorldPosition{ get; set; }
        public Vector3 WorldPositionGround{ get; set; }
        public int Height{ get; set; }
        
        public int SplinePointIndex{ get; set; }
        
        public  float Size{ get; set; }
        public  Color Color{ get; set; }
       

        public SplinePointModel(Vector3 worldPosition,Vector3 worldPositionGround, int height, float size, Color color, int splinePointIndex)
        {
            WorldPosition = worldPosition;
            WorldPositionGround = worldPositionGround;
            Height = height;
            Size = size;
            Color = color;
            SplinePointIndex = splinePointIndex;
        }
    
    }
}

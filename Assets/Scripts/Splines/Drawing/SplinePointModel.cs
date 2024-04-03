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
        
        public Vector3 SplinePointForward { get; set; }
       

        public SplinePointModel(Vector3 worldPositionGround,Vector3 worldPosition, int height, float size, Color color, int splinePointIndex)
        {
            WorldPosition = worldPosition;
            WorldPositionGround = worldPositionGround;
            Height = height;
            Size = size;
            Color = color;
            SplinePointIndex = splinePointIndex;
        }
        
        public SplinePointModel(Vector3 worldPositionGround, float size, int splinePointIndex)
        {
            WorldPositionGround = worldPositionGround;
            Size = size;
            SplinePointIndex = splinePointIndex;
        }
        
        public SplinePointModel Clone()
        {
            return new SplinePointModel(WorldPositionGround, WorldPosition, Height, Size, Color, SplinePointIndex);
        }
    
    }
}

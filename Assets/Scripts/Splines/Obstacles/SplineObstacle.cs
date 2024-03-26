using UnityEngine;
namespace Splines.Obstacles
{
    public class SplineObstacle : MonoBehaviour
    {
        [Range(-5, 5)]
        [SerializeField]
        private float _obstacleSpeedInfluence;
        
        public float ObstacleSpeedInfluence => _obstacleSpeedInfluence;
    }
}

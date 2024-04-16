using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// Move Properties used by <see cref="ActionMoveData"/>
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/MoveSystem/Properties")]
    public partial class ActionMoveProperties : ScriptableObject
    {
        public float Speed = 10;
        public float AngularSpeed = 100;
        public float Acceleration = 100;
        public float StopDistance = 0;
    }
}
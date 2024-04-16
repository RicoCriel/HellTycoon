using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// Perform Properties used by <see cref="ActionPerformData"/>
    /// This is bare bones at the moment but in the future may include Action Delay, a list of animation to trigger etc.
    /// If you wish to extend it, feel free to just inherit it and cast it in the mean time.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Actions/Properties")]
    public partial class ActionPerformProperties : ScriptableObject
    {
        public float Duration = 1.0f;
    }
}
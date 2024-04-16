using System;
using UnityEngine;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The base data used by <see cref="MoveSystemBase"/> to set destination and properties.
    /// This isn't a scriptable object because DestinationTransform would not be able to reference a scene object.
    /// This class is partial so you can extend it with more properties.
    /// </summary>
    [Serializable]
    public partial class ActionMoveData
    {
        public bool Required;
        public Transform DestinationTransform;
        public ActionMoveProperties Properties;
    }
}
using System;

namespace TinnyStudios.AIUtility
{
    /// <summary>
    /// The performing data for <see cref="UtilityAction"/>.
    /// This class is partial so you can extend it with more properties.
    /// </summary>
    [Serializable]
    public partial class ActionPerformData
    {
        public ActionPerformProperties Properties;
    }
}
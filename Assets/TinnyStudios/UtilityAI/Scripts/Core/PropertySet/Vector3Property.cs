using System;
using UnityEngine;

namespace TinnyStudios.AIUtility.Core.Properties
{
    /// <summary>
    /// A concrete implementation of property of Vector3.
    /// For it to show in Unity Inspector, it needs to be concrete.
    /// </summary>
    [Serializable]
    public class Vector3Property : Property<Vector3>
    {
    }
}
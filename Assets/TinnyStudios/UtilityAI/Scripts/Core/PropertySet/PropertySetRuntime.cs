using System;
using UnityEngine;

namespace TinnyStudios.AIUtility.Core.Properties
{
    /// <summary>
    /// The object containing an instance of <see cref="PropertySet{TProperty,TValue}"/> used by <see cref="Agent"/> and <see cref="UtilityAction"/>
    /// This class provides quick access to Property Set, Set and Add.
    /// </summary>
    [Serializable]
    public partial class PropertySetRuntime
    {
        [Header("Leave empty if you do not want to use a property set.")]
        public PropertySetAsset Original;

        [SerializeField]
        private PropertySetAsset _instance;

        public bool Clone = true;

        public bool Initialized => GetInstance() != null;

        public PropertySetAsset GetInstance()
        {
            if (_instance == null)
                Initialize();

            return _instance;
        }

        public void Initialize()
        {
            if (Original == null)
                return;

            Original.Initialize();
            _instance = Clone ? Original.Clone() : Original;
        }

        public void Set(string key, float value) => GetInstance().FloatPropertySet.SetValue(key, value);
        public void Set(string key, Vector3 value) => GetInstance().Vector3PropertySet.SetValue(key, value);
        public void Set(string key, bool value) => GetInstance().BoolPropertySet.SetValue(key, value);
        public void Set(string key, int value) => GetInstance().IntPropertySet.SetValue(key, value);
        public void Set(string key, Transform value) => GetInstance().TransformPropertySet.SetValue(key, value);

        public bool TryAdd(string key, float value) => GetInstance().FloatPropertySet.TryAdd(key, value);
        public bool TryAdd(string key, Vector3 value) => GetInstance().Vector3PropertySet.TryAdd(key, value);
        public bool TryAdd(string key, bool value) => GetInstance().BoolPropertySet.TryAdd(key, value);
        public bool TryAdd(string key, int value) => GetInstance().IntPropertySet.TryAdd(key, value);
        public bool TryAdd(string key, Transform value) => GetInstance().TransformPropertySet.TryAdd(key, value);
        public bool TryGet(string key, out float value) => GetInstance().FloatPropertySet.TryGet(key, out value);
        public bool TryGet(string key, out Vector3 value) => GetInstance().Vector3PropertySet.TryGet(key, out value);
        public bool TryGet(string key, out bool value) => GetInstance().BoolPropertySet.TryGet(key, out value);
        public bool TryGet(string key, out int value) => GetInstance().IntPropertySet.TryGet(key, out value);
        public bool TryGet(string key, out Transform value) => GetInstance().TransformPropertySet.TryGet(key, out value);
    }
}
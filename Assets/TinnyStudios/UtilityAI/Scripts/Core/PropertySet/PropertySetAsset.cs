using UnityEngine;

namespace TinnyStudios.AIUtility.Core.Properties
{
    /// <summary>
    /// The scriptable object containing a collection of property sets that is used by <see cref="Agent"/> and <see cref="UtilityAction"/>
    /// This is a useful way of keeping track of the game states and passing it down to the Consideration class or other systems without touching the code base.
    /// </summary>
    [CreateAssetMenu(menuName = "TinnyStudios/UtilityAI/Properties/Property Set")]
    public partial class PropertySetAsset : ScriptableObject
    {
        public FloatPropertySet FloatPropertySet = new FloatPropertySet();
        public Vector3PropertySet Vector3PropertySet = new Vector3PropertySet();
        public BoolPropertySet BoolPropertySet = new BoolPropertySet();
        public IntPropertySet IntPropertySet = new IntPropertySet();
        public TransformPropertySet TransformPropertySet = new TransformPropertySet();

        private bool _initialized;

        protected virtual void Awake()
        {
            _initialized = false;
        }

        partial void InitializeAdditionalProperties();

        /// <summary>
        /// Initializes all property sets. 
        /// </summary>
        public virtual void Initialize()
        {
            if (_initialized)
                return;

            FloatPropertySet.Initialize();
            Vector3PropertySet.Initialize();
            BoolPropertySet.Initialize();
            IntPropertySet.Initialize();
            TransformPropertySet.Initialize();

            InitializeAdditionalProperties();

            _initialized = true;
        }

        /// <summary>
        /// Clones the current set to return a new instance. This is so the original scriptable object is not modified.
        /// </summary>
        /// <returns>A new instance of property set asset.</returns>
        public virtual PropertySetAsset Clone()
        {
            var propertySet = Instantiate(this);
            propertySet.Initialize();
            return propertySet;
        }
    }


}
using System;
using System.Collections.Generic;

namespace TinnyStudios.AIUtility.Core.Properties
{
    /// <summary>
    /// A generic property that handles storing the value of the property.
    /// The name is used in the lookup table.
    /// </summary>
    [Serializable]
    public class Property<T> : IProperty<T>
    {
        public string Name;
        public T Value;

        public Property()
        {
        }

        public Property(T value)
        {
            Value = value;
        }

        public virtual void SetValue(T value)
        {
            Value = value;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetValue<T1>(T1 value)
        {
        }

        string IProperty<T>.Name => Name;
        T IProperty<T>.Value => Value;
    }

    /// <summary>
    /// A generic property set that handles storing and creating a look up for quick access.
    /// </summary>
    /// <typeparam name="TProperty">The concrete property such as <see cref="FloatProperty"/></typeparam>
    /// <typeparam name="TValue">A value such as <see cref="float"/></typeparam>
    public class PropertySet<TProperty, TValue> : IPropertySet where TProperty : IProperty<TValue>, new()
    {
        public List<TProperty> Properties = new List<TProperty>();
        public Dictionary<string, TProperty> PropertyLookUp = new Dictionary<string, TProperty>();

        /// <summary>
        /// Initialize the LookUp table from a list of properties.
        /// </summary>
        public void Initialize()
        {
            foreach (var property in Properties)
            {
                if(!PropertyLookUp.ContainsKey(property.Name))
                    PropertyLookUp.Add(property.Name, property);
            }
        }

        /// <summary>
        /// Try to add a new property name with a value.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns>True if the look up doesn't contain the property name, False if it does.</returns>
        public bool TryAdd(string propertyName, TValue  value)
        {
            if (PropertyLookUp.ContainsKey(propertyName)) 
                return false;

            var property = new TProperty();
            property.SetName(propertyName);
            property.SetValue(value);

            Properties.Add(property);
            PropertyLookUp.Add(property.Name, property);
            return true;
        }

        /// <summary>
        /// Set the value of a property name. It will try to add if it doesn't exist.
        /// </summary>
        /// <param name="propertyName">Name of property to add.</param>
        /// <param name="value">Starting value of the property</param>
        public void SetValue(string propertyName, TValue value)
        {
            if (TryGetProperty(propertyName, out var property))
                property.SetValue(value);
            else
                TryAdd(propertyName, value);
        }

        /// <summary>
        /// Tries to get the property class. This isn't not suggested to use, <see cref="TryGet"/>
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="property">The class containing the property value and name</param>
        /// <returns>True if successful</returns>
        public bool TryGetProperty(string propertyName, out TProperty property)
        {
            return PropertyLookUp.TryGetValue(propertyName, out property);
        }

        /// <summary>
        /// Try to get the value of a property.
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <param name="value">The value of the property</param>
        /// <returns>True if successful</returns>
        public bool TryGet(string propertyName, out TValue value)
        {
            if (TryGetProperty(propertyName, out var property))
            {
                value = property.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }
    }

    /// <summary>
    /// A generic interface for a property to be used in <see cref="PropertySet{TProperty,TValue}"/>
    /// It stores and set a value. That can be found through a table on Property Set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProperty<T>
    {
        string Name { get; }

        T Value { get; }

        void SetValue(T value);
        void SetName(string name);
    }

    public interface IPropertySet
    {
        void Initialize();
    }
}
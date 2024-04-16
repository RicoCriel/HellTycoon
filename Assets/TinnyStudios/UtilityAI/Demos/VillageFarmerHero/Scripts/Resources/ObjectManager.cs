using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// The generic manager for a <see cref="ManagerObject{TManager}"/>
    /// This add a way to create a TreeManager of TreeObjects. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    public abstract class ObjectManager<T, TManager> : MonoBehaviour
        where T : MonoBehaviour, IManagerObject<TManager>
        where TManager : class
    {
        public List<T> Objects = new List<T>();
        public T Prefab;

        public TManager ConcreteManager { get; private set; }

        public int Count => Objects.Count;

        private void Awake()
        {
            BindAllDependencies();
        }

        private void BindAllDependencies()
        {
            ConcreteManager = this as TManager;

            var objs = GetComponentsInChildren<T>();
            foreach (var obj in objs)
                obj.Bind(ConcreteManager);

            Objects = objs.ToList();
        }

        private void BindDepdency(T obj)
        {
            obj.Bind(ConcreteManager);
        }

        public void Add(T obj)
        {
            Objects.Add(obj);
            BindDepdency(obj);
        }

        public void Remove(T obj)
        {
            Objects.Remove(obj);
        }

        public T GetFirstOrDefault()
        {
            return Objects.FirstOrDefault();
        }

        public T Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (parent == null)
                parent = transform;

            var instance = Instantiate(Prefab, parent) as T;
            instance.transform.position = position;
            instance.transform.rotation = rotation;

            Add(instance);

            return instance;
        }
    }
}
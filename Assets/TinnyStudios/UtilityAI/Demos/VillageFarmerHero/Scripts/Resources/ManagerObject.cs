using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// The generic object of TManager. This allows you to create a TreeManager for Trees.
    /// </summary>
    /// <typeparam name="TManager">The class implementing ObjectManager. </typeparam>
    public abstract class ManagerObject<TManager> : MonoBehaviour, IManagerObject<TManager>
    {
        public TManager Manager { get; private set; }

        public virtual void Bind(TManager manager)
        {
            Manager = manager;
        }
    }
}
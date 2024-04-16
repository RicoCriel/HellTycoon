using UnityEngine;

namespace TinnyStudios.AIUtility.Impl.Examples.FarmerHero
{
    /// <summary>
    /// A component to be attached to an object to define it as a tree.
    /// Place the game object under a parent with <see cref="TreeObjectManager"/> and it'll be added to the manager.
    /// </summary>
    public class TreeObject : ManagerObject<TreeObjectManager>
    {
        public bool Taken;

        [ContextMenu("Chop")]
        public void OnChopped()
        {
            Manager.Remove(this);
            gameObject.SetActive(false);
        }
    }
}
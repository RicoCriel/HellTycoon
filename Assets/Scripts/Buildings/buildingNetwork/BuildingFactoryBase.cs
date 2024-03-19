using Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Buildings
{
    public class BuildingFactoryBase : BuildingBase
    {
        private Queue<GameObject> demoncontrainer = new Queue<GameObject>();
        
        [SerializeField]
        private int MaxDemons = 10;
        
        private int MachineSpawnRatePerSecond = 1;
        
        public bool ContainerHasSpace()
        {
            return demoncontrainer.Count < MaxDemons;
        }
        
        public void AddDemon(GameObject demon)
        {
            demoncontrainer.Enqueue(demon);
        }
        
        public GameObject GetDemon()
        {
            return demoncontrainer.Dequeue();
        }

        public void SpawnDemon(PlaceholderConnectorHitBox OutNode)
        {
           //if next machione is not full
           
           //if this machine still has demons
              if (OutNode.SpawnObject(demoncontrainer.Peek()))
              {
                demoncontrainer.Dequeue();
              }
              
              
        }

    }
}

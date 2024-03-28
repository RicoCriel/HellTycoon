using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Buildings
{
    public class MachineManager : MonoBehaviour
    {
        private List<GameObject> _machines = new List<GameObject>();

        public void AddMachine()
        {

        }

        public void RemoveMachine(int index)
        {
            _machines.RemoveAt(index);
        }

        // Set current machine as parent
        public void AttachToCurrentMachine(MachinePart machinePart)
        {

        }

        public void AttachToCurrentMachine(BuildingFactoryBase building)
        {

        }

        private void SelectMachineWithPart(BuildingFactoryBase building)
        {

        }
    }

    public enum MachineType
    {
        //TODO: change names
        Type0 = 0,
        Type1 = 1,
        Type2 = 2
    }

}
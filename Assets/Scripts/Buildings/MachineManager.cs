using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

namespace Buildings
{
    public class MachineManager : MonoBehaviour
    {
        private List<GameObject> _machines = new List<GameObject>();
        public List<GameObject> Machines => _machines;

        public void AddMachine(GameObject machine)
        {
            _machines.Add(machine);
        }

        public void RemoveMachine(int index)
        {
            _machines.RemoveAt(index);
        }
        
        public int SumUpkeep()
        {
            int sum = 0;
            List<MachinePart> _machineParts = _machines.ConvertAll(machine => machine.GetComponent<MachinePart>());
            foreach (var machinePart  in _machineParts)
            {
                sum += machinePart.UkpeepCost;
            }
            return sum;
        }
    
    }

    public enum MachineType
    {
        Type0 = 0,
        Type1 = 1,
        Type2 = 2
    }

}
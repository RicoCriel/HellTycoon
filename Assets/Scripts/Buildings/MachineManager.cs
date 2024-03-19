using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Buildings
{
    public class MachineManager : MonoBehaviour
    {
        [SerializeField] private Machine _machinePrefab;
        private List<GameObject> _machines = new List<GameObject>();

        public void AddMachine(MachineInput input)
        {
            var machine = Instantiate(_machinePrefab);
            machine.name = "Machine " + _machines.Count;
            machine.transform.SetParent(transform, false);
            machine.Initialize(input, _machines.Count);
            _machines.Add(machine.gameObject);
        }

        public void removeMachine(int index)
        {
            _machines.RemoveAt(index);
        }
    }

    enum MachineType : int
    {
        //TODO: change names

        Type0 = 0,
        Type1 = 1,
        Type2 = 2
    }

}
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Buildings
{
    public class MachineManager : MonoBehaviour
    {
        [SerializeField] private Machine _machinePrefab;
        private List<GameObject> _machines = new List<GameObject>();
        private Machine _currentMachine;
        public Machine CurrentMachine => _currentMachine;

        public void AddMachine()
        {
            var machine = Instantiate(_machinePrefab);
            machine.name = "Machine " + _machines.Count;
            machine.transform.SetParent(transform, false);
            machine.Initialize(_machines.Count);
            _machines.Add(machine.gameObject);
            _currentMachine = machine;
        }

        public void RemoveMachine(int index)
        {
            _machines.RemoveAt(index);
        }

        public void FinishMachine(GameObject output)
        {
            _currentMachine = null;
        }

        public bool CanBuildMachinePart()
        {
            return _currentMachine != null;
        }

        // Set current machine as parent
        public void AttachToCurrentMachine(GameObject go)
        {
            if (_currentMachine != null)
            {
                go.transform.parent = _currentMachine.transform;
            }
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
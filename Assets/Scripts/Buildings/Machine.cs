using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Buildings
{
    public class Machine : MonoBehaviour
    {
        [SerializeField] private MachineNode _startNode;
        private List<MachinePart> _machineParts;
        private IEnumerator _coroutine;
        private MachineOutput _output;
        private int _machineIndex;

        public int MachineIndex => _machineIndex;

        private void Awake()
        {
            _machineParts = new List<MachinePart>();
        }

        public void Initialize(int index)
        {
            _machineIndex = index;
        }

        public void AddMachinePart(MachinePart machinePart)
        {
            Assert.IsTrue(machinePart);
            if (_machineParts == null) return;

            var currentNode = _machineParts.Count == 0 ? _startNode : _machineParts[^1].Node;

            if (currentNode == null) return;

            if (currentNode.NextNodes.Count <= 0) return;

            machinePart.Initialize(currentNode.NextNodes[(int)machinePart.MachineType], _machineIndex,
                _machineParts.Count - 1);
            //machinePart.OnDestruct.AddListener(RemoveMachinePart);
            _machineParts.Add(machinePart);
        }

        public void RemoveMachinePartAt(int idx)
        {
            if (_machineParts == null || _machineParts.Count <= idx) return;

            _machineParts.RemoveAt(idx);

            // reset nodes
            MachineNode currentNode = _startNode;
            for (int i = 0; i != _machineParts.Count; ++i)
            {
                _machineParts[i].Node = currentNode.NextNodes[(int)_machineParts[i].MachineType];
                currentNode = _machineParts[i].Node;
            }
        }

        public void RemoveMachinePart(BuildingFactoryBase building)
        {
            if (building.TryGetComponent(out MachinePart machinePart))
            {
                RemoveMachinePartAt(machinePart.PartIndex);
            }
        }

        public MachinePart GetMachinePartAt(int idx)
        {
            return _machineParts[idx];
        }
    }
}



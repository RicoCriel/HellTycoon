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

        private void Awake()
        {
            _machineParts = new List<MachinePart>();
            //_machineParts.Add(_startNode);

            //_coroutine = Produce();
        }

        public void Initialize(int index)
        {
            _machineIndex = index;
        }

        //public void SetOutput(MachineOutput output)
        //{
        //    _output = output;
        //    output.Node = _machineParts[^1];
        //}

        public void AddMachine(MachinePart machinePart)
        {
            Assert.IsTrue(machinePart);
            if (_machineParts == null) return;

            var currentNode = _machineParts.Count == 0 ? _startNode : _machineParts[^1].Node;

            if (currentNode == null) return;

            if (currentNode.NextNodes.Count <= 0) return;

            machinePart.Node = currentNode.NextNodes[(int)machinePart.MachineType];
            _machineParts.Add(machinePart);

            // TODO: remove
            //currentNode = currentNode.NextNodes[(int)machineTypeIdx];
            //Debug.Log("Stat 0: " + currentNode.Stat0 + " Stat 3: " + currentNode.Stat3);
        }

        public void RemoveMachine()
        {
            if (_machineParts == null || _machineParts.Count <= 1) return;

            _machineParts.RemoveAt(_machineParts.Count - 1);

            //// TODO: remove
            //var currentNode = _machineParts[^1];
            //Debug.Log("Stat 0: " + currentNode.Stat0 + " Stat 3: " + currentNode.Stat3);
        }

        //public MachineNode GetCurrentNode()
        //{
        //    return _machineParts[^1];
        //}

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //    AddMachine(0);

            //if (Input.GetKeyDown(KeyCode.R))
            //    RemoveMachine();
        }

        public void StartProduction()
        {
            //StartCoroutine(_coroutine);
        }

        //IEnumerator Produce()
        //{
        //    yield return new WaitForSeconds(_machineParts[^1].ProcessTime);

        //    _output.SpawnDemon(_machineParts[^1]);

        //}

        public MachinePart GetMachineAt(int idx)
        {
            return _machineParts[idx];
        }
    }
}



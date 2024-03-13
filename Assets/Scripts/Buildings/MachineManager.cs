using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private MachineNode _currentNode;

    public void AddMachine(int machineIdx)
    {
        if (_currentNode == null) return;

        if (_currentNode.NextNodes.Count <= machineIdx) return;

        _currentNode = _currentNode.NextNodes[machineIdx];

        Debug.Log("Stat 0: " + _currentNode.Stat0 + " Stat 3: " + _currentNode.Stat3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            AddMachine(0);
    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private MachineNode _startNode;
    private List<MachineNode> _machines;
    private IEnumerator _coroutine;
    private MachineOutput _output;
    private MachineInput _input;

    private void Awake()
    {
        _machines = new List<MachineNode>();
        _machines.Add(_startNode);

        _coroutine = Produce();
    }

    public void Initialize(MachineInput input)
    {
        _input = input;
    }

    public void SetOutput(MachineOutput output)
    {
        _output = output;
    }

    public void AddMachine(int machineTypeIdx)
    {
        if (_machines == null) return;

        var currentNode = _machines[^1];

        if (currentNode == null) return;

        if (currentNode.NextNodes.Count <= machineTypeIdx) return;

        _machines.Add(currentNode.NextNodes[machineTypeIdx]);

        // TODO: remove
        currentNode = currentNode.NextNodes[machineTypeIdx];
        Debug.Log("Stat 0: " + currentNode.Stat0 + " Stat 3: " + currentNode.Stat3);
    }

    public void RemoveMachine()
    {
        if(_machines == null || _machines.Count <= 1) return;

        _machines.RemoveAt(_machines.Count - 1);

        // TODO: remove
        var currentNode = _machines[^1];
        Debug.Log("Stat 0: " + currentNode.Stat0 + " Stat 3: " + currentNode.Stat3);
    }

    public MachineNode GetCurrentNode()
    {
        return _machines[^1];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            AddMachine(0);

        if(Input.GetKeyDown(KeyCode.R))
            RemoveMachine();
    }

    public void StartProduction()
    {
        StartCoroutine(_coroutine);
    }

    IEnumerator Produce()
    {
        yield return new WaitForSeconds(_machines[^1].ProcessTime);

        _output.SpawnDemon(_machines[^1]);
        _input.Open = true;
    }
}

enum MachineType : int
{
    //TODO: change names

    Type0 = 0,
    Type1 = 1,
    Type2 = 2
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineNode : ScriptableObject
{
    [SerializeField] private List<MachineNode> _nextNodes;
    public List<MachineNode> NextNodes => _nextNodes;

    [SerializeField] private int _stat0;
    public int Stat0 => _stat0;

    [SerializeField] private int _stat1;
    public int Stat1 => _stat1;

    [SerializeField] private int _stat2;
    public int Stat2 => _stat2;

    [SerializeField] private int _stat3;
    public int Stat3 => _stat3;

    [SerializeField] private float _processTime;
    public float ProcessTime => _processTime;
}


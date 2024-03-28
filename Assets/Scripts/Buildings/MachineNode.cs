using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachineNode : ScriptableObject
{
    [SerializeField] private List<MachineNode> _nextNodes;
    public List<MachineNode> NextNodes => _nextNodes;

    [SerializeField] private int _body;
    public int Body => _body;

    [SerializeField] private int _wings;
    public int Wings => _wings;

    [SerializeField] private int _horns;
    public int Horns => _horns;

    [SerializeField] private int _armor;
    public int Armor => _armor;

    [SerializeField] private int _face;
    public int Face => _face;

    [SerializeField] private float _processTime;
    public float ProcessTime => _processTime;
}


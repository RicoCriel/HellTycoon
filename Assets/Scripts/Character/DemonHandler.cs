using System;
using Buildings;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHandler : MonoBehaviour
{
    [SerializeField] private DemonStatsInt _maxLevel;

    [SerializeField] private MeshFilter _horns;
    [SerializeField] private MeshFilter _head;
    [SerializeField] private MeshFilter _face;
    [SerializeField] private MeshFilter _armor;
    [SerializeField] private MeshFilter _wings;
    //[SerializeField] private MeshFilter _wingsL;

    [SerializeField] private Mesh[] _hornsMeshes;
    [SerializeField] private Mesh[] _headMeshes;
    [SerializeField] private Mesh[] _faceMeshes;
    [SerializeField] private Mesh[] _armorMeshes;
    [SerializeField] private Mesh[] _wingsMeshes;

    private Camera _mainCamera;

    public DemonStatsInt Level;

    [SerializeField] private MachineNode _currentMachineNode;


    private void Start()
    {
        Level.Body = 0;
        Level.Face = 0;
        Level.Wings = 0;
        Level.Horn = 0;
        Level.Armor = 0;

    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        //AngleToCamera();
    }

    private void OnEnable()
    {
        UpdateSprites();
    }


    private void Update()
    {
        //AngleToCamera();
    }

    private void UpdateSprites()
    {
        if (_maxLevel.Body < Level.Body) { Level.Body = _maxLevel.Body - 1; }
        if (_maxLevel.Face < Level.Face) { Level.Face = _maxLevel.Face - 1; }
        if (_maxLevel.Horn < Level.Horn) { Level.Horn = _maxLevel.Horn - 1; }
        if (_maxLevel.Armor < Level.Armor) { Level.Armor = _maxLevel.Armor - 1; }
        if (_maxLevel.Wings < Level.Wings) { Level.Wings = _maxLevel.Wings - 1; }

        _horns.sharedMesh = _hornsMeshes[Level.Horn];
        //Debug.Log(_horns.sprite);
        _head.sharedMesh = _headMeshes[Level.Body];
        //_face.sharedMesh = _faceMeshes[Level.Face];
        _armor.sharedMesh = _armorMeshes[Level.Armor];
        _wings.sharedMesh = _wingsMeshes[Level.Wings];
        //_wingsL.sharedMesh = _wingsMeshes[Level.Wings];

    }

    private void AngleToCamera()
    {
        if (_mainCamera != null)
        {
            Vector3 lookAtDirection = _mainCamera.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(lookAtDirection);
        }
        else
        {
            Debug.LogWarning("No camera assigned");
        }

    }

    public void SetStats(MachineType machineType)
    {
        _currentMachineNode = _currentMachineNode.NextNodes[(int)machineType];

        Level.Body = _currentMachineNode.Body;
        Level.Wings = _currentMachineNode.Wings;
        Level.Horn = _currentMachineNode.Horns;
        Level.Armor = _currentMachineNode.Armor;
        Level.Face = _currentMachineNode.Face;

    }

}


[Serializable]
public struct DemonStatsInt
{
    public int Body;
    public int Horn;
    public int Wings;
    public int Armor;
    public int Face;

    public DemonStatsInt(int body, int horn, int wings, int armor, int face)
    {
        Body = body;
        Horn = horn;
        Wings = wings;
        Armor = armor;
        Face = face;
    }

    public DemonStatsInt(int value)
    {
        Body = value;
        Horn = value;
        Wings = value;
        Armor = value;
        Face = value;
    }
}

[Serializable]
public struct DemonStatsFloat
{
    public float Body;
    public float Horn;
    public float Wings;
    public float Armor;
    public float Face;

    public DemonStatsFloat(float body, float horn, float wings, float armor, float face)
    {
        Body = body;
        Horn = horn;
        Wings = wings;
        Armor = armor;
        Face = face;
    }

    public DemonStatsFloat(float value)
    {
        Body = value;
        Horn = value;
        Wings = value;
        Armor = value;
        Face = value;
    }
}



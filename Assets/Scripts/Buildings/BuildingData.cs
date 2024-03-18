using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string _displayName;
    public string DisplayName => _displayName;

    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField] private GameObject _prefab;
    public GameObject Prefab => _prefab;

    [SerializeField] private Vector2Int _buildingSize;
    public Vector2Int BuildingSize => _buildingSize;

    [SerializeField] private int _price;
    public int Price => _price;

    [SerializeField] private PartType _partType;
    public PartType PartType => _partType;

    [SerializeField] private string _buildTag;
    public string BuildTag => _buildTag;
}

public enum PartType
{
    Manufacturing = 0,
    Power = 1,
    Logistics = 2
}
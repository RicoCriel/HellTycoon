using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildingData : ScriptableObject
{
    [SerializeField] string _displayName;
    public string DisplayName => _displayName;

    [SerializeField] Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField] GameObject _prefab;
    public GameObject Prefab => _prefab;

    [SerializeField] Vector2Int _buildingSize;
    public Vector2Int BuildingSize => _buildingSize;

    [SerializeField] private int _price;
    public int Price => _price;

}
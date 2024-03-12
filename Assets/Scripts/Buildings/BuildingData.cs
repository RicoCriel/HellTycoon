using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildingData : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;
    public GameObject Prefab;
    public Vector2Int BuildingSize;
    [SerializeField] private float _processTime;
    public float ProcessTime => _processTime;
    [SerializeField] private int _price;
    public int Price => _price;

}
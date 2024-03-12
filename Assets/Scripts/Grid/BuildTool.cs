using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class BuildTool : MonoBehaviour
{
    [SerializeField] private GameObject _currentBuilding;

    public void Build(Tile tile)
    {
        if (_currentBuilding == null) return;
        Instantiate(_currentBuilding, tile.transform.position, Quaternion.identity);
    }
}

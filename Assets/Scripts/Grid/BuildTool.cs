using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Grid
{
    public class BuildTool : MonoBehaviour
    {
        [SerializeField] private GameObject _currentBuilding;

        public void Build(Transform tile)
        {
            if (_currentBuilding == null) return;
            Instantiate(_currentBuilding, tile.transform.position, Quaternion.identity);
        }
    }
}


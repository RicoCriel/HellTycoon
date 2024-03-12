using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hoverMaterial;
        [SerializeField] private Material _selectedMaterial;

        private TileGrid _grid;
        private Material _currentMaterial;
        private Renderer _renderer;
        private Vector2Int _gridPosition;

        public bool Occupied = false;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _currentMaterial = _defaultMaterial;
        }

        public void Initialize(TileGrid grid, Vector2Int gridPosition)
        {
            _grid = grid;
            _gridPosition = gridPosition;
        }

        public void SetOccupied()
        {
            if(Occupied) return;

            _renderer.material = _selectedMaterial;
            _currentMaterial = _selectedMaterial;
            Occupied = true;
        }

        private void OnMouseEnter()
        {
            if (Occupied) return;
            _renderer.material = _hoverMaterial;
        }

        private void OnMouseExit()
        {
            if (Occupied) return;
            _renderer.material = _currentMaterial;
        }

        private void OnMouseDown()
        {
            if (Occupied) return;

            SetOccupied();

            Tile tile;
            if (_grid.GetTileAtGridPosition(_gridPosition.x + 1, _gridPosition.y, out tile))
            {
                tile.SetOccupied();
            }
        }
    }
}


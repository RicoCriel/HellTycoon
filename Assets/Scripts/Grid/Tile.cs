using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        private UnityEvent<Tile> _buildEvent;

        private bool _occupied = false;
        public bool Occupied => _occupied;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _currentMaterial = _defaultMaterial;

            _buildEvent = new UnityEvent<Tile>();
        }

        public void Initialize(TileGrid grid, Vector2Int gridPosition, BuildTool buildTool)
        {
            _grid = grid;
            _gridPosition = gridPosition;
            
            _buildEvent.AddListener(buildTool.Build);
        }

        public void SetOccupied()
        {
            if (_occupied) return;

            _renderer.material = _selectedMaterial;
            _currentMaterial = _selectedMaterial;
            _occupied = true;
        }

        private void OnMouseEnter()
        {
            if (_occupied) return;
            _renderer.material = _hoverMaterial;
        }

        private void OnMouseExit()
        {
            if (_occupied) return;
            _renderer.material = _currentMaterial;
        }

        private void OnMouseDown()
        {
            if (_occupied) return;

            //TODO: remove
            SetOccupied();

            _buildEvent.Invoke(this);
        }
    }
}


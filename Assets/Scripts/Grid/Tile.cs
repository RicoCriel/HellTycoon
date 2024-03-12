using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Grid
{
    public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hoverMaterial;
        [SerializeField] private Material _selectedMaterial;

        private TileGrid _grid;
        private Material _currentMaterial;
        private Renderer _renderer;
        private Vector2Int _gridPosition;
        private UnityEvent<Transform> _buildEvent;

        private bool _occupied = false;
        public bool Occupied => _occupied;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _currentMaterial = _defaultMaterial;

            _buildEvent = new UnityEvent<Transform>();
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_occupied) return;
            _renderer.material = _hoverMaterial;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_occupied) return;
            _renderer.material = _currentMaterial;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_occupied) return;

            //TODO: remove
            SetOccupied();

            var childTrans = transform.GetChild(0);
            if (childTrans != null)
                _buildEvent.Invoke(childTrans);
        }
    }
}


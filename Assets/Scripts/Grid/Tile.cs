using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class Tile : MonoBehaviour
    {
        private TileGrid _grid;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hoverMaterial;
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void Initialize(TileGrid grid)
        {
            _grid = grid;
        }

        private void OnMouseEnter()
        {
            _renderer.material = _hoverMaterial;
        }

        private void OnMouseExit()
        {
            _renderer.material = _defaultMaterial;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class TileGrid : MonoBehaviour
    {
        [SerializeField] private int _width, _height;
        [SerializeField] private Tile _tilePrefab;

        private Dictionary<Vector2Int, Tile> _tiles;
        private LayerManager _layerManager;

        public void Initialize(int width, int height, LayerManager manager, Tile tilePrefab)
        {
            _width = width;
            _height = height;
            _layerManager = manager;

            GenerateGrid(tilePrefab);
        }

        private void GenerateGrid(Tile tilePrefab)
        {
            if (_tiles == null)
                _tiles = new Dictionary<Vector2Int, Tile>();
            else
                _tiles.Clear();

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Tile spawnedTile = Instantiate(tilePrefab, transform, false);
                    spawnedTile.transform.Translate(x, 0, y);

                    
                    spawnedTile.name = "Tile [" + x + ", " + y + "]";
                    spawnedTile.Initialize(this);

                    _tiles.Add(new Vector2Int(x, y), spawnedTile);
                }
            }
        }
    }
}


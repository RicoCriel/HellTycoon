using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LayerManager : MonoBehaviour
    {
        [SerializeField] private Tile _tilePrefab;
        [SerializeField] private TileGrid _layerPrefab;
        [SerializeField] private int _layerCount;
        [SerializeField] private int _layerWidth;
        [SerializeField] private int _layerHeight;
        [SerializeField] private BuildTool _buildTool;
       

        private List<TileGrid> _layers;
        public List<TileGrid> Layers => _layers;

        private void Awake()
        {
            _layers = new List<TileGrid>();
            for (int i = 0; i != _layerCount; ++i)
            {
                AddLayer();
            }
        }

        private void AddLayer()
        {
            int idx = _layers.Count;
            var newLayer = Instantiate(_layerPrefab, Vector3.zero, Quaternion.identity);
            newLayer.transform.Translate(_layerWidth * idx, idx, transform.position.z);

            newLayer.Initialize(_layerWidth, _layerHeight, this, _tilePrefab, idx, _buildTool);
            newLayer.name = "Layer " + idx;
            _layers.Add(newLayer);
        }

        private void Update()
        {
            //TODO: remove
            if (Input.GetKeyDown(KeyCode.F))
            {
                AddLayer();
            }
        }
    }
}


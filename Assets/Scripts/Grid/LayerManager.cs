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

        private List<TileGrid> _layers;

        void Awake()
        {
            _layers = new List<TileGrid>();
            for (int i = 0; i != _layerCount; ++i)
            {
                var newLayer = Instantiate(_layerPrefab, Vector3.zero, Quaternion.identity);
                newLayer.transform.Translate(_layerWidth * i, i, transform.position.z);

                newLayer.Initialize(_layerWidth, _layerHeight, this, _tilePrefab);
                newLayer.name = "Layer " + i;
                _layers.Add(newLayer);
            }
        }

    }
}


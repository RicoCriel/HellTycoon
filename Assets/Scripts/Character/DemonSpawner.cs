using Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] GameObject _demonPrefab;
    [SerializeField] float _spawnInterval = 5f;
    [SerializeField] float _maxOffset = 0.5f;
    [SerializeField] int _spawnCost = 5;
    [SerializeField] EconManager _econManager;
    private float _timeSinceLastSpawn = 0f;
    [SerializeField] private PlaceholderConnectorHitBox _connector;

 

        void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            if (_econManager.GetMoney() >= _spawnCost)
            {
                _timeSinceLastSpawn = 0f;

                _econManager.SubtractMoney(_spawnCost);
                float offsetX = Random.Range(-_maxOffset, _maxOffset);
                float offsetZ = Random.Range(-_maxOffset, _maxOffset);
                Vector3 offset = new Vector3(offsetX, 0, offsetZ);

                if(_demonPrefab == null) return;
                var demon = Instantiate(_demonPrefab, transform.position + offset, Quaternion.identity);
                _connector.SpawnObject(demon);

                
            }
        }
    }
    

   
    
}

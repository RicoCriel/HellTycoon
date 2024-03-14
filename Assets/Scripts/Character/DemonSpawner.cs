using Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] GameObject DemonPrefab;
    [SerializeField] float SpawnInterval = 5f;
    [SerializeField] float maxOffset = 0.5f;
    [SerializeField] int SpawnCost = 5;
    [SerializeField] EconManager _econManager;
    private float _timeSinceLastSpawn = 0f;
    [SerializeField] private PlaceholderConnectorHitBox _connector;

 

        void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= SpawnInterval)
        {
            if (_econManager.GetMoney() >= SpawnCost)
            {
                _econManager.SubtractMoney(SpawnCost);
                float offsetX = Random.Range(-maxOffset, maxOffset);
                float offsetZ = Random.Range(-maxOffset, maxOffset);
                Vector3 offset = new Vector3(offsetX, 0, offsetZ);

                var demon = Instantiate(DemonPrefab, transform.position + offset, Quaternion.identity);
                _connector.SpawnObject(demon);

                _timeSinceLastSpawn = 0f;
            }
        }
    }
    

   
    
}

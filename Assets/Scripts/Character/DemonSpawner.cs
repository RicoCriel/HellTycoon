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

    private List<GameObject> _demonHandler;

    private void Start()
    {
        _demonHandler = new List<GameObject>();
    }

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

                if (_demonPrefab == null) return;
                var demon = Instantiate(_demonPrefab, transform.position + offset, Quaternion.identity);

                //if (!_connector.SpawnObject(demon))
                //{
                //    _demonHandler.Add(demon);
                //}
                //else
                //{
                //    StartCoroutine(MoveDemonsToConnector());
                //}
                _demonHandler.Add(demon);
                StartCoroutine(MoveDemonsToConnector());
            }
        }
    }

    IEnumerator MoveDemonsToConnector()
    {
        for (int i = 0; i < _demonHandler.Count; ++i)
        {
            GameObject demon = _demonHandler[i];
            Vector3 targetPosition = _connector.transform.position;

            while (Vector3.Distance(demon.transform.position, targetPosition) > 0.1f)
            {
                demon.transform.position = Vector3.MoveTowards(demon.transform.position, targetPosition, Time.deltaTime * 2);
                yield return null;
            }

            // At this point, the demon has reached the connector
            // Put the demon in the connector
            _connector.SpawnObject(demon);
            _demonHandler.RemoveAt(i);
        }

        //_demonHandler.Clear();
    }
}

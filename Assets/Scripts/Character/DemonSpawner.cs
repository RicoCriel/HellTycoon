using Splines;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Economy;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _demonPrefab;
    [SerializeField] public float SpawnInterval = 5f;
    [SerializeField] private float _maxOffset = 0.5f;
    [SerializeField] private int _spawnCost = 5;
    [SerializeField] private EconomyManager _economyManager;
    [SerializeField] private PlaceholderConnectorHitBox _connector;
    [SerializeField] private DemonManager _demonManager;
    private float _timeSinceLastSpawn = 0f;



    private List<GameObject> _demonHandler;

    private void Awake()
    {
        _demonHandler = new List<GameObject>();
        if (_demonManager == null)
        {
            _demonManager = FindObjectOfType<DemonManager>();
        }

        if (_economyManager == null)
        {
            _economyManager = FindObjectOfType<EconomyManager>();
        }
    }

    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= SpawnInterval && _connector.ImConnected)
        {
            //_demonHandler.Add(demon);
            if (_connector.Spline.EndConnector.myBuildingNode.TryGetComponent(out BuildingFactoryBase nextMachine))
            {
                if (nextMachine.BuildingHasSpace())
                {
                    _timeSinceLastSpawn = 0f;

                    _economyManager.AutoCost(_spawnCost);
                    float offsetX = Random.Range(-_maxOffset, _maxOffset);
                    float offsetZ = Random.Range(-_maxOffset, _maxOffset);
                    Vector3 offset = new Vector3(offsetX, 0, offsetZ);

                    if (_demonPrefab == null) return;
                    GameObject demon = Instantiate(_demonPrefab, transform.position + offset, Quaternion.identity);
            
                    if (_demonManager == null) { Debug.Log("Demon Manager is null"); return; }
                    _demonManager.AddDemon(demon);

                    //StartCoroutine(MoveDemonToConnector(demon));

                    if (!_connector.SpawnObject(demon))
                    {
                        Destroy(demon);
                    }
                }
            }
            
        }
    }
    
    IEnumerator MoveDemonToConnector(GameObject demon)
    {
        Vector3 targetPosition = _connector.transform.position;

        while (Vector3.Distance(demon.transform.position, targetPosition) > 0.1f)
        {
            demon.transform.position = Vector3.MoveTowards(demon.transform.position, targetPosition, Time.deltaTime * 2);
            yield return null;
        }

        // At this point, the demon has reached the connector
        // Put the demon in the connector
        if (!_connector.SpawnObject(demon))
        {
            Destroy(demon);
        }
    }
}

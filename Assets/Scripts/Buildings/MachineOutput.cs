using System.Collections;
using System.Collections.Generic;
using Splines;
using UnityEngine;

public class MachineOutput : MonoBehaviour
{
    [SerializeField] private PlaceholderConnectorHitBox _connector;
    [SerializeField] private GameObject _demonPrefab;

    private void Awake()
    {
        if (_connector == null)
            _connector = GetComponent<PlaceholderConnectorHitBox>();
    }

    public void SpawnDemon(MachineNode node)
    {
        if (_demonPrefab == null) return;

        var demon = Instantiate(_demonPrefab, transform.position, Quaternion.identity);
        var handler = demon.GetComponent<DemonHandler>();

        if (handler == null) return;

        handler.HornLevel = node.Stat0;
        handler.FaceLevel = node.Stat1;
        handler.WingsLevel = node.Stat2;
        handler.ArmorLevel = node.Stat3;

        _connector.SpawnObject(demon);
    }
}

using Buildings;
using System.Collections;
using System.Collections.Generic;
using Economy;
using UnityEngine;

public class FearMachine : BuildingFactoryBase
{
    [SerializeField] private float _drainRate;
    [SerializeField] private float _fearRate;
    [SerializeField] private float _fearApplyInterval;
    [SerializeField] private float _soulToSoulPowerRate;
    [SerializeField] private int _layertHightDiff = 100;
    [SerializeField] private DemonStatsFloat _fearValue = new DemonStatsFloat(10f);

    private DemonManager _demonManager;
    private EconomyManager _economyManager;
    private int _layer;

    private void Start()
    {
        InvokeRepeating("ApplyFear", 1f, _fearApplyInterval);
        if (_demonManager == null)
        {
            _demonManager = GameObject.FindObjectOfType<DemonManager>();
            _economyManager = GameObject.FindObjectOfType<EconomyManager>();
        }
        _layer = (int)(transform.position.y) / _layertHightDiff;
    }

    private float DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        float sum = 5f + demoncomp.Level.Horn * _fearValue.Horn +
                    demoncomp.Level.Body * _fearValue.Body +
                    demoncomp.Level.Face * _fearValue.Face +
                    demoncomp.Level.Armor * _fearValue.Armor +
                    demoncomp.Level.Wings * _fearValue.Wings;

        return sum;
    }

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0)
        {
            foreach (var demon in _unprocessedDemonContainer)
            {
                _fearRate += DemonValue(demon) * _soulToSoulPowerRate;

                Destroy(demon);
            }
            _unprocessedDemonContainer.Clear();
        }

    }


    private void ApplyFear()
    {
        for (int i = 0; i < _demonManager.GetDemonFears().Count; i++)
        {
            DemonFear demon = _demonManager.GetDemonFears()[i];
            if (demon.Layer == _layer)
            {
                demon.IncreaseFear(_fearRate);
            }
        }
    }

    protected override void ExecuteMachineSpawningBehaviour()
    {
        return;
    }
}

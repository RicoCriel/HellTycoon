using Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearMachine : BuildingFactoryBase
{
    [SerializeField] private float _drainRate;
    [SerializeField] private float _fearRate;
    [SerializeField] private float _fearApplyInterval;
    [SerializeField] private float _soulToSoulPowerRate;
    [SerializeField] private int _layertHightDiff = 100;
    private DemonManager _demonManager;
    private SoulManager _soulManager;
    private int _layer;

    private void Start()
    {
        InvokeRepeating("ApplyFear", 1f, _fearApplyInterval);
        if (_demonManager == null)
        {
            _demonManager = GameObject.FindObjectOfType<DemonManager>();
            _soulManager = GameObject.FindObjectOfType<SoulManager>();
        }
        _layer = (int)(transform.position.y) / _layertHightDiff;
    }

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = 5 + demoncomp.Level.Horn * _soulManager.HornLevelValue +
                    demoncomp.Level.Body * _soulManager.BodyLevelValue +
                        demoncomp.Level.Face * _soulManager.FaceLevelValue +
                            demoncomp.Level.Armor * _soulManager.ArmorLevelValue +
                                demoncomp.Level.Wings * _soulManager.WingLevelValue;

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

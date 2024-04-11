using Buildings;
using System.Collections;
using System.Collections.Generic;
using Economy;
using UnityEngine;

public class PlayerDemon : BuildingFactoryBase
{
    [SerializeField] private float _starveApplyInterval;
    [SerializeField] private float _starveRate;
    [SerializeField] private float _baseFearRate;
    [SerializeField] private float _feedSize;
    [SerializeField] private float _fearApplyInterval;
    [SerializeField] private float _soulToSoulPowerRate;
    [SerializeField] private DemonStatsFloat _fearValue = new DemonStatsFloat(10f);

    private DemonManager _demonManager;
    private EconomyManager _economyManager;

    private void Start()
    {
        InvokeRepeating("Starve", 1f, _starveApplyInterval);
        InvokeRepeating("ApplyFear", 1f, _fearApplyInterval);
        if (_demonManager == null)
        {
            _demonManager = GameObject.FindObjectOfType<DemonManager>();
        }
        if (_economyManager == null)
        {
            _economyManager = GameObject.FindObjectOfType<EconomyManager>();

        }


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
                _baseFearRate += DemonValue(demon) * _soulToSoulPowerRate;

                Destroy(demon);
            }
            _unprocessedDemonContainer.Clear();
        }

    }

    private void Starve()
    {
        _feedSize -= _starveRate;
        if(_feedSize <= 0.0001f)
            {
            _feedSize = 0.0001f;
        }
        ScaleWithFeed();
    }

    private void ScaleWithFeed()
    {
        float feedSizeScaled = (_feedSize / 10);
        transform.localScale = new Vector3(feedSizeScaled, feedSizeScaled, feedSizeScaled);

    }

    private void ApplyFear()
    {
        for (int i = 0; i < _demonManager.GetDemonFears().Count; i++)
        {
            _demonManager.GetDemonFears()[i].IncreaseFear(_baseFearRate * (int)_feedSize);
        }
    }

    protected override void ExecuteMachineSpawningBehaviour()
    {
        return;
    }
}

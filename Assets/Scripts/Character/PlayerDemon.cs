using Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemon : BuildingFactoryBase
{
    [SerializeField] private float _starveApplyInterval;
    [SerializeField] private float _starveRate;
   [SerializeField] private float _baseFearRate;
    [SerializeField] private float _feedSize;
    [SerializeField] private float _fearApplyInterval;
    [SerializeField] private float _soulToSoulPowerRate;
    private DemonManager _demonManager;
    private EconManager _econManager;

    private void Start()
    {
        InvokeRepeating("Starve", 1f, _starveApplyInterval);
        InvokeRepeating("ApplyFear", 1f, _fearApplyInterval);
        if(_demonManager == null)
        {
            _demonManager = GameObject.FindObjectOfType<DemonManager>();
            _econManager = GameObject.FindObjectOfType<EconManager>();
        }
        
    }

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = 5 + demoncomp.Level.Horn * _econManager.HornLevelValue +
                    demoncomp.Level.Body * _econManager.BodyLevelValue +
                        demoncomp.Level.Face * _econManager.FaceLevelValue +
                            demoncomp.Level.Armor * _econManager.ArmorLevelValue +
                                demoncomp.Level.Wings * _econManager.WingLevelValue;

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
}

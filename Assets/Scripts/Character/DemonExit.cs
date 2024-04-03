using System.Collections;
using System.Collections.Generic;
using Buildings;
using Economy;
using UnityEngine;

public class DemonExit : BuildingFactoryBase
{
    [SerializeField] private SoulManager _soulManager;

    // TODO: move market to econmanager and remove demonvalue function
    [SerializeField] private Market _market;

    private void Awake()
    {
        if (_market == null)
        {
            _market = FindObjectOfType<Market>();
        }
    }

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = 5 + demoncomp.Level.Horn * _soulManager.HornLevelValue +
                    demoncomp.Level.Body * _soulManager.BodyLevelValue +
                        demoncomp.Level.Face * _soulManager.FaceLevelValue +
                            demoncomp.Level.Armor * _soulManager.ArmorLevelValue +
                                demoncomp.Level.Wings * _soulManager.WingLevelValue;

        _market.SupplyDemon(demoncomp.Level);

        return sum;
    }

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0)
        {
            foreach (var demon in _unprocessedDemonContainer)
            {
                _soulManager.AddMoney(DemonValue(demon));

                Destroy(demon);
            }
            _unprocessedDemonContainer.Clear();
        }

    }

    protected override void ExecuteMachineSpawningBehaviour()
    {
        return;
    }
}

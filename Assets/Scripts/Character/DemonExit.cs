using System.Collections;
using System.Collections.Generic;
using Buildings;
using Economy;
using UnityEngine;

public class DemonExit : BuildingFactoryBase
{
    [SerializeField] private EconManager _econManager;

    // TODO: move market to econmanager and remove demonvalue function
    [SerializeField] private Market _market;

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = 5 + demoncomp.Level.Horn * _econManager.HornLevelValue +
                    demoncomp.Level.Body * _econManager.BodyLevelValue +
                        demoncomp.Level.Face * _econManager.FaceLevelValue +
                            demoncomp.Level.Armor * _econManager.ArmorLevelValue +
                                demoncomp.Level.Wings * _econManager.WingLevelValue;

        _market.SupplyDemon(demoncomp.Level);

        return sum;
    }

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0)
        {
            foreach (var demon in _unprocessedDemonContainer)
            {
                _econManager.AddMoney(DemonValue(demon));

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

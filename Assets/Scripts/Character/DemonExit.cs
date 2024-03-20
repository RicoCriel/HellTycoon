using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class DemonExit : BuildingFactoryBase
{
    [SerializeField] private EconManager _econManager;

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = 5 + demoncomp.HornLevel * _econManager.HornLevelValue +
                    demoncomp.BodyLevel * _econManager.BodyLevelValue +
                        demoncomp.FaceLevel * _econManager.FaceLevelValue +
                            demoncomp.ArmorLevel * _econManager.ArmorLevelValue +
                                demoncomp.WingsLevel * _econManager.WingLevelValue;
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

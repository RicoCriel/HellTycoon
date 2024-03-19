using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

public class DemonExit : BuildingFactoryBase
{
    [SerializeField] private EconManager _econManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null) return;

        string tag = other.tag;
        if (tag == "Demon")
        {
            _econManager.AddMoney(DemonValue(other.gameObject));
            Destroy(other.gameObject);
        }
    }

    private int DemonValue(GameObject devil)
    {
        var demoncomp = devil.GetComponent<DemonHandler>();

        int sum = demoncomp.HornLevel * _econManager.HornLevelValue +
                    demoncomp.BodyLevel * _econManager.BodyLevelValue +
                        demoncomp.FaceLevel * _econManager.FaceLevelValue +
                            demoncomp.ArmorLevel * _econManager.ArmorLevelValue +
                                demoncomp.WingsLevel * _econManager.WingLevelValue;
        return sum;
    }

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0)
            _unprocessedDemonContainer.Clear();

    }

    protected override void ExecuteMachineSpawningBehaviour()
    {
        return;
    }
}

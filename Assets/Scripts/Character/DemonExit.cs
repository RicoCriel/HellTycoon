using System.Collections;
using System.Collections.Generic;
using Buildings;
using Economy;
using UnityEngine;

public class DemonExit : BuildingFactoryBase
{
    [SerializeField] private EconomyManager _economyManager;

    private void Awake()
    {
        if (_economyManager == null)
        {
            _economyManager = FindObjectOfType<EconomyManager>();
        }
    }

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0)
        {
            foreach (var demon in _unprocessedDemonContainer)
            {
                if (demon.TryGetComponent(out DemonHandler demonHandler))
                    _economyManager.SellDemon(demonHandler.Level);

                Destroy(demon);
            }
            _unprocessedDemonContainer.Clear();
        }
    }
}

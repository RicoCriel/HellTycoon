using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSettings : MonoBehaviour
{
    private float _upgradeValueModi;
    public float UpgradeValueModi
    {
        get
        {
            return _upgradeValueModi;
        }
        set 
        {
            _upgradeValueModi = value;
        }
    }

    private float _soulPriceModi;
    public float SoulPriceModi
    {
        get
        {
            return _soulPriceModi;
        }
        set
        {
            _soulPriceModi = value;
        }
    }

    private float _neededFearMulti;
    public float NeededFearMulti
    {
        get
        {
            return _neededFearMulti;
        }
        set
        {
            _neededFearMulti = value;
        }
    }

    private bool _extaFuelModi;
    public bool ExtraFuelModi
    {
        get
        {
            return _extaFuelModi;
        }
        set
        {
            _extaFuelModi = value;
        }
    }

    private bool _betterSoldier;
    public bool BetterSoldier
    {
        get
        {
            return _betterSoldier;
        }
        set
        {
            _betterSoldier = value;
        }
    }

}

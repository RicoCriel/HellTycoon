using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private EconManager _economyManager;
    [SerializeField] private Text _moneyText;


    private void Awake()
    {
        if (_economyManager == null)
        {
            _economyManager = GameObject.FindObjectOfType<EconManager>();
        }
    }
    void Update()
    {
        _moneyText.text = "Money: " + _economyManager.GetMoney();
        
    }
}

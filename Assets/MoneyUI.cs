using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUi : MonoBehaviour
{
    [SerializeField] EconManager _economyManager;
    [SerializeField] Text _moneyText;
    void Update()
    {
        _moneyText.text = "Money: " + _economyManager.GetMoney();
        
    }
}

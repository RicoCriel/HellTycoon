using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] EconManager economyManager;
    [SerializeField] Text moneyText;
    void Update()
    {
        moneyText.text = "Money: " + economyManager.GetMoney();
    }
}

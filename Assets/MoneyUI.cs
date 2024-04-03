using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private SoulManager _economyManager;
    [SerializeField] private Text _moneyText;


    private void Awake()
    {
        if (_economyManager == null)
        {
            _economyManager = GameObject.FindObjectOfType<SoulManager>();
        }
    }
    void Update()
    {
        _moneyText.text = "Money: " + _economyManager.GetMoney();
        
    }
}

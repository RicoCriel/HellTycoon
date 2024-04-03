using System.Collections;
using System.Collections.Generic;
using Economy;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private SoulManager _soulManager;
    [SerializeField] private Text _moneyText;


    private void Awake()
    {
        if (_soulManager == null)
        {
            _soulManager = GameObject.FindObjectOfType<SoulManager>();
        }
    }
    void Update()
    {
        _moneyText.text = "Money: " + _soulManager.Money;
        
    }
}

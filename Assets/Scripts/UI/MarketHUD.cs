using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Economy;
using TMPro;
using UnityEngine;

public class MarketHUD : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _pricesList = new List<TMP_Text>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMarketStat(StatType stat, float price)
    {
        if (_pricesList.Count > (int)stat)
        {
            _pricesList[(int)stat].text = price.ToString("F", CultureInfo.CurrentCulture);
        }
    }
}

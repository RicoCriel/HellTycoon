using System.Collections;
using System.Collections.Generic;
using Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YearHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _yearNumber;

    private string _yearStr;

    private void Awake()
    {
        _yearStr = _yearNumber.text;
    }

    public void SetYear(int year)
    {
        _yearNumber.text = _yearStr + year.ToString();
    }
}

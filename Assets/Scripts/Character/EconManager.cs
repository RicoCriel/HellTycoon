using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconManager : MonoBehaviour
{
  
    
   


    [SerializeField] private int _startMoney = 200;
    [SerializeField] private bool _logMoney = false;

    private int _money = 0;

    public int HornLevelValue = 100;
    public int BodyLevelValue = 150;
    public int FaceLevelValue = 200;
    public int ArmorLevelValue = 250;
    public int WingLevelValue = 300;
    void Start()
    {
        _money = _startMoney;
    }
    public int GetMoney()
    {
        return _money;
    }
    public void AddMoney(int amount)
    {
        _money += amount;
        if (_logMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }

    public void SubtractMoney(int amount)
    {
        _money -= amount;
        if (_logMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }

   
    }
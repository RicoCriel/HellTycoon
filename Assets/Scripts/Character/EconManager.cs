using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconManager : MonoBehaviour
{
  
    
    public int HornLevelValue = 100;
    public int BodyLevelValue = 150;
    public int FaceLevelValue = 200;
    public int ArmorLevelValue = 250;
    public int WingLevelValue = 300;


    [SerializeField] int StartMoney = 200;
    [SerializeField] bool LogMoney = false;

    private int _money = 0;
    void Start()
    {
        _money = StartMoney;
    }
    public int GetMoney()
    {
        return _money;
    }
    public void AddMoney(int amount)
    {
        _money += amount;
    }

    public void SubtractMoney(int amount)
    {
        _money -= amount;
    }

    private void Update()
    {
        if(LogMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }
    }
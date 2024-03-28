using Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconManager : MonoBehaviour
{
   


    [SerializeField] private int _startMoney = 200;
    [SerializeField] private bool _logMoney = false;
    [SerializeField] private bool _godMode = false;
    [SerializeField] private MachineManager _machineManager;

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

    public void PayUpkeep()
    {
        //_machineManager.Machines;
        //loop over all machines
        //check and sum price ukeep cost for each macihne
        //pay upkeep (call this fucntion every x seconds)       
    }

    public void SubtractMoney(int amount)
    {
        if (_godMode) return;

        _money -= amount;
        if (_logMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }

   
    }
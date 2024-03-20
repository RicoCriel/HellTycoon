using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    [SerializeField] private int _fuelCapacity = 100;
    [SerializeField] private int _currFuel = 5;
    // Update is called once per frame


    public bool AddFuel(int addedFuel)
    {
        if (_fuelCapacity > _currFuel)
        {
            _currFuel += addedFuel;
            //make sure doenst go above max cap
            if(_currFuel > _fuelCapacity)
            {
                _currFuel = _fuelCapacity;
            }
            //return true if fuel was added
            return true;
        }
        //return false if fuel was full
        else { return false; }
    }

    public bool RemoveFuel(int fuelUsed)
    {
        if (_currFuel >= fuelUsed)
        {
            _currFuel -= fuelUsed;
            //return true if fuel was used
            return true;
        }
        //return false if fuel wasnt used (not engouh fuel left)
        else { return false; }
    }

}

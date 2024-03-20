using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelGen : BasicMachine
{
    [SerializeField] private int _fuelGenRate = 1;
    [SerializeField] private int _fuelGenAmount = 1;
    [SerializeField] private int _fuelGenMax = 100;
    [SerializeField] private int _fuelGenCurr = 0;
    [SerializeField] private FuelSystem _fuelSystem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateFuel());
    }

    private IEnumerator GenerateFuel()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fuelGenRate);
            if (_fuelGenCurr < _fuelGenMax)
            {
                _fuelGenCurr += _fuelGenAmount;
                _fuelSystem.AddFuel(_fuelGenAmount);
            }
        }
    }
}

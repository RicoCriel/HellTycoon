using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Economy
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private DemonStatsFloat _demand = new DemonStatsFloat(10f);

        // Threshold to switch from positive price change to negative
        [SerializeField] private int _demandThreshold;

        [SerializeField] private float _supplySaturation;
        private float _demandEventModifier;
        private float _wealth = 1f;

        [SerializeField] private DemonStatsInt _supply;
        [SerializeField] private DemonStatsFloat _prices;
        [Space(15)]
        [SerializeField] private float _supplyTime = 10f;

        private void Update()
        {
            // TODO: remove test code
            if (Input.GetKeyDown(KeyCode.F))
            {
                var stats = new DemonStatsInt(1, 1, 1, 1, 1);
                SupplyDemon(stats);
            }
        }

        public void CalculateDemand()
        {
            _demand.Wings = (1 - 1 / (1 + _supplySaturation * _supply.Wings) * _wealth * _demandEventModifier);
            _demand.Horn = (1 - 1 / (1 + _supplySaturation * _supply.Horn) * _wealth * _demandEventModifier);
            _demand.Armor = (1 - 1 / (1 + _supplySaturation * _supply.Armor) * _wealth * _demandEventModifier);
            _demand.Body = (1 - 1 / (1 + _supplySaturation * _supply.Body) * _wealth * _demandEventModifier);
            _demand.Face = (1 - 1 / (1 + _supplySaturation * _supply.Face) * _wealth * _demandEventModifier);
        }

        public void SupplyDemon(DemonStatsInt demon)
        {
            _supply.Wings += demon.Wings;
            _supply.Horn += demon.Horn;
            _supply.Face += demon.Face;
            _supply.Body += demon.Body;
            _supply.Armor += demon.Armor;

            Debug.Log("Wings: " + _supply.Wings + " Horn: " + _supply.Horn + "  Face: " + _supply.Face + "  Body: " +
                      _supply.Body + "  Armor: " + _supply.Armor);

            StartCoroutine(RemoveFromSupply(demon));
        }

        public void BoostDemand(DemonStatsFloat demon, float time)
        {
            _demand.Wings += demon.Wings;
            _demand.Horn += demon.Horn;
            _demand.Face += demon.Face;
            _demand.Body += demon.Body;
            _demand.Armor += demon.Armor;

            StartCoroutine(RemoveFromDemand(demon, time));
        }

        private IEnumerator RemoveFromDemand(DemonStatsFloat demon, float time)
        {
            yield return new WaitForSeconds(time);

            _demand.Wings -= demon.Wings;
            _demand.Horn -= demon.Horn;
            _demand.Face -= demon.Face;
            _demand.Body -= demon.Body;
            _demand.Armor -= demon.Armor;
        }

        private IEnumerator RemoveFromSupply(DemonStatsInt demon)
        {
            yield return new WaitForSeconds(_supplyTime);

            _supply.Wings -= demon.Wings;
            _supply.Horn -= demon.Horn;
            _supply.Face -= demon.Face;
            _supply.Body -= demon.Body;
            _supply.Armor -= demon.Armor;

            Debug.Log("Wings: " + _supply.Wings + " Horn: " + _supply.Horn + "  Face: " + _supply.Face + "  Body: " +
                      _supply.Body + "  Armor: " + _supply.Armor);

        }
    }
}



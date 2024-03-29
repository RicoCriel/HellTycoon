using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Economy
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private DemonHandler.DemonStats _demand = new DemonHandler.DemonStats(10);
        [SerializeField] private DemonHandler.DemonStats _supply;
        [SerializeField] private DemonHandler.DemonStats _prices;
        [Space(15)]
        [SerializeField] private float _supplyTime = 10f;

        private void Update()
        {
            // TODO: remove test code
            if (Input.GetKeyDown(KeyCode.F))
            {
                var stats = new DemonHandler.DemonStats(1, 1, 1, 1, 1);
                SupplyDemon(stats);
            }
        }

        public void SupplyDemon(DemonHandler.DemonStats demon)
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

        public void BoostDemand(DemonHandler.DemonStats demon, float time)
        {
            _demand.Wings += demon.Wings;
            _demand.Horn += demon.Horn;
            _demand.Face += demon.Face;
            _demand.Body += demon.Body;
            _demand.Armor += demon.Armor;

            StartCoroutine(RemoveFromDemand(demon, time));
        }

        private IEnumerator RemoveFromDemand(DemonHandler.DemonStats demon, float time)
        {
            yield return new WaitForSeconds(time);

            _demand.Wings -= demon.Wings;
            _demand.Horn -= demon.Horn;
            _demand.Face -= demon.Face;
            _demand.Body -= demon.Body;
            _demand.Armor -= demon.Armor;
        }

        private IEnumerator RemoveFromSupply(DemonHandler.DemonStats demon)
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



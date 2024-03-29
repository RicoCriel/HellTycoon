using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Economy
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private DemonHandler.DemonStats _demand;
        [SerializeField] private DemonHandler.DemonStats _supply;
        [SerializeField] private DemonHandler.DemonStats _prices;
        [Space(15)]
        [SerializeField] private float _supplyTime = 10f;

        private void Update()
        {
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

            yield break;
        }
    }

}



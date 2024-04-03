using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private Market _market;
        [SerializeField] private SoulManager _soulManager;

        private void Awake()
        {
            if (_market == null)
            {
                _market = FindObjectOfType<Market>();
            }

            if (_soulManager == null)
            {
                _soulManager = FindObjectOfType<SoulManager>();
            }
        }

        private void Update()
        {
            // TODO: remove debug code
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartDemandEventModifier(StatType.Wings, 1.5f, 10f);
            }
        }

        public void SellDemon(DemonStatsInt demon)
        {
            _soulManager.AddMoney(_market.CalculateDemonPrice(demon));
            _market.SupplyDemon(demon);
        }

        public bool BuyObject(float amount)
        {
            if (_soulManager.Money > 0f)
            {
                _soulManager.SubtractMoney(amount);
                return true;
            }
            return false;
        }

        // Use for payments that happen automatically(upkeep, spawning...)
        public void AutoCost(float amount)
        {
            _soulManager.SubtractMoney(amount);
        }

        public void StartDemandEventModifier(StatType statType, float modifier, float time)
        {
            _market.AddModifier(statType, modifier, time);
        }
    }

}


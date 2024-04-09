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
        [SerializeField] private TimeManager _timeManager;

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

            if (_timeManager == null)
            {
                _timeManager = FindObjectOfType<TimeManager>();
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
            var price = _market.CalculateDemonPrice(demon);
            _soulManager.AddMoney(price);
            _market.SupplyDemon(demon);
            _timeManager.AddTransaction(price, TransactionType.Sale);
        }

        public bool BuyObject(float amount)
        {
            if (_soulManager.Money > 0f)
            {
                _soulManager.SubtractMoney(amount);
                _timeManager.AddTransaction(amount, TransactionType.Investment);
                return true;
            }
            return false;
        }

        // Use for payments that happen automatically(upkeep, spawning...)
        public void AutoCost(float amount)
        {
            _soulManager.SubtractMoney(amount);
            _timeManager.AddTransaction(amount, TransactionType.Upkeep);
        }

        public void StartDemandEventModifier(StatType statType, float modifier, float time)
        {
            _market.AddModifier(statType, modifier, time);
        }
    }


    public enum TransactionType
    {
        Sale,
        Investment,
        Bonus,
        Upkeep
    }
}


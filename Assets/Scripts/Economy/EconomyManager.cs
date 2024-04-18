using System.Collections;
using System.Collections.Generic;
using Tycoons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static ContractSystem;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private Market _market;
        [FormerlySerializedAs("_soulManager")]
        [SerializeField] private SoulManager _playerSoulManager;
        [FormerlySerializedAs("_timeManager")]
        [SerializeField] private TimeManager _playerTimeManager;

        [SerializeField] private SoulManager _soulManagerPrefab;

        private Dictionary<TycoonType, SoulManager> _tycoonSoulManagers = new Dictionary<TycoonType, SoulManager>();

        private void Awake()
        {
            if (_market == null)
            {
                _market = FindObjectOfType<Market>();
            }

            //since we nstanciate the tycoons we can still use findobjectoftype for the player instances

            if (_playerSoulManager == null)
            {
                _playerSoulManager = FindObjectOfType<SoulManager>();
            }

            if (_playerTimeManager == null)
            {
                _playerTimeManager = FindObjectOfType<TimeManager>();
            }
        }

        public void SetupTycoonEconomy(Tycoon tycoon)
        {
            SoulManager soulManager = Instantiate(_soulManagerPrefab, tycoon.transform);

            if (!_tycoonSoulManagers.TryAdd(tycoon.TycoonType, soulManager))
            {
                Debug.Log("Tycoon of this type already registered.");
                Destroy(soulManager.gameObject);
                return;
            }

            soulManager.Init(tycoon.TycoonData);
            soulManager.OnLost += tycoon.LoseLogic;

            HookUpTycoonEvents(tycoon);
        }

        private void HookUpTycoonEvents(Tycoon tycoon)
        {
            //tycoon.SellTriggered += (s, e) => SellDemon(tycoon);
            //tycoon.BuyTriggered += (s, e) => BuyObject(tycoon);
            //tycoon.AutoCostTriggered += (s, e) => AutoCost(tycoon);
        }

        private void Update()
        {
            // TODO: remove debug code
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    StartDemandEventModifier(StatType.Wings, 1.5f, 10f);
            //}
        }

        //Selling
        public void SellDemon(DemonStatsInt demon)
        {
            ContractSystem.UpdateConProgress(ContractType.SellCon, 1);
            float price = _market.CalculateDemonPrice(demon);
            _playerSoulManager.AddMoney(price);
            ContractSystem.UpdateConProgress(ContractType.EarnCon, (int)price);
            _market.SupplyDemon(demon);

            // _playerTimeManager.AddTransaction(price, TransactionType.Sale);
            _playerTimeManager.AddTransaction(price, TransactionType.Sale);

        }

        public void SellDemon(Tycoon tycoon)
        {
            //List<DemonStatsInt> sales = tycoon.AIBehaviour.SellBehaviour(this, tycoon);

            //foreach (DemonStatsInt demon in sales)
            //{
            //    float price = _market.CalculateDemonPrice(demon);
            //    _tycoonSoulManagers[tycoon.TycoonType].AddMoney(price);
            //    _market.SupplyDemon(demon);
            //}
        }

        public void SellDemon(DemonStatsInt demon, TycoonType tycoon)
        {
            float price = _market.CalculateDemonPrice(demon);
            _tycoonSoulManagers[tycoon].AddMoney(price);
            _market.SupplyDemon(demon);
        }

        //Buying
        public bool BuyObject(float amount)
        {
            if (_playerSoulManager.Money > 0f)
            {
                Debug.Log("buying for: " + amount);
                _playerSoulManager.SubtractMoney(amount);
                _playerTimeManager.AddTransaction(amount, TransactionType.Investment);

                return true;
            }
            return false;
        }

        public bool BuyObject(float amount, TycoonType tycoon)
        {
            if (_tycoonSoulManagers[tycoon].Money > 0f)
            {
                _tycoonSoulManagers[tycoon].SubtractMoney(amount);

                return true;
            }

            return false;
        }

        //Upkeep
        public void AutoCost(float amount)
        {
            _playerSoulManager.SubtractMoney(amount);
            _playerTimeManager.AddTransaction(amount, TransactionType.Upkeep);
        }

        public void AutoCost(Tycoon tycoon)
        {
            //float autoCostAmount = tycoon.AIBehaviour.AutoCostBehaviour(this, tycoon);
            //_tycoonSoulManagers[tycoon.TycoonType].SubtractMoney(autoCostAmount);
        }

        public void StartDemandEventModifier(StatType statType, float modifier, float time)
        {
            _market.AddModifier(statType, modifier, time);
        }

        public float GetDemand(StatType statType)
        {
            return _market.GetDemand(statType);
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

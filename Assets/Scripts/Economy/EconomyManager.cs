using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Economy
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private Market _market;
        [FormerlySerializedAs("_soulManager")]
        [SerializeField] private SoulManager _playerSoulManager;
        [FormerlySerializedAs("_timeManager")]
        [SerializeField] private TimeManager _playerTimeManager;

        [SerializeField]
        private Tycoon tycoonPrefab;

        [SerializeField]
        private List<TycoonData> _thisGameTycoons = new List<TycoonData>();
        private Dictionary<tycoonType, AIBehaviourBase> _aiBehaviours = new Dictionary<tycoonType, AIBehaviourBase>();

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

            InitTycoonBehaviours();
            InitTycoons();
        }
        private void InitTycoonBehaviours()
        {
            _aiBehaviours.Add(tycoonType.tycoonOne, new AIBehaviourTycoon1());
            _aiBehaviours.Add(tycoonType.tycoonTwo, new AIBehaviourTycoon2());
            _aiBehaviours.Add(tycoonType.tycoonThree, new AIBehaviourTycoon3());
        }
        private void InitTycoons()
        {
            foreach (TycoonData tycoonType in _thisGameTycoons)
            {
                Tycoon tycoon = Instantiate(tycoonPrefab, transform);
                tycoon.gameObject.name = tycoonType.ToString();
                if (InitializeTycoon(tycoonType, tycoon)) return;
                HookUpTycoonEvents(tycoon);
            }
        }

        private bool InitializeTycoon(TycoonData tycoonType, Tycoon tycoon)
        {

            if (_aiBehaviours.TryGetValue(tycoonType._tycoonType, out AIBehaviourBase aiBehaviour))
            {
                tycoon.Init(tycoonType, aiBehaviour);
            }
            else
            {
                Debug.LogError("AI Behaviour not found");
                return true;
            }
            return false;
        }
        private void HookUpTycoonEvents(Tycoon tycoon)
        {
            tycoon.SellTriggered += (s, e) => SellDemon(tycoon);
            tycoon.BuyTriggered += (s, e) => BuyObject(tycoon);
            tycoon.AutoCostTriggered += (s, e) => AutoCost(tycoon);
        }

        private void Update()
        {
            // TODO: remove debug code
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartDemandEventModifier(StatType.Wings, 1.5f, 10f);
            }
        }

        //Selling
        public void SellDemon(DemonStatsInt demon)
        {
            float price = _market.CalculateDemonPrice(demon);
            _playerSoulManager.AddMoney(price);
            _market.SupplyDemon(demon);

            // _playerTimeManager.AddTransaction(price, TransactionType.Sale);
            _playerTimeManager.CurrentYear.AddTransaction(price, TransactionType.Sale);

        }

        public void SellDemon(Tycoon tycoon)
        {
            List<DemonStatsInt> sales = tycoon.AIBehaviour.SellBehaviour(this, _market, tycoon.SoulManager, tycoon);

            foreach (DemonStatsInt demon in sales)
            {
                float price = _market.CalculateDemonPrice(demon);
                Debug.Log("DemonPrice: " + price);
                tycoon.SoulManager.AddMoney(price);
                _market.SupplyDemon(demon);
                tycoon.TimeManager.CurrentYear.AddTransaction(price, TransactionType.Sale);
            }

        }

        //Buying
        public bool BuyObject(float amount)
        {
            if (_playerSoulManager.Money > 0f)
            {
                Debug.Log("buying for: " + amount);
                _playerSoulManager.SubtractMoney(amount);
                _playerTimeManager.CurrentYear.AddTransaction(amount, TransactionType.Investment);

                return true;
            }
            return false;
        }

        public bool BuyObject(Tycoon tycoon)
        {
            float buyAmount = tycoon.AIBehaviour.BuyBehaviour(this, _market, tycoon.SoulManager, tycoon);

            if (tycoon.SoulManager.Money > 0f)
            {
                tycoon.SoulManager.SubtractMoney(buyAmount);
                tycoon.TimeManager.CurrentYear.AddTransaction(buyAmount, TransactionType.Investment);
                return true;
            }

            return false;
        }

        //Upkeep
        public void AutoCost(float amount)
        {


            _playerSoulManager.SubtractMoney(amount);
            _playerTimeManager.CurrentYear.AddTransaction(amount, TransactionType.Upkeep);
        }

        public void AutoCost(Tycoon tycoon)
        {
            float autoCostAmount = tycoon.AIBehaviour.AutoCostBehaviour(this, _market, tycoon.SoulManager, tycoon);

            tycoon.SoulManager.SubtractMoney(autoCostAmount);
            tycoon.TimeManager.CurrentYear.AddTransaction(autoCostAmount, TransactionType.Upkeep);

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

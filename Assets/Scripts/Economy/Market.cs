using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Economy
{
    // Should only be referenced by economymanager
    public class Market : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<StatType, MarketStat> _marketStats = new SerializedDictionary<StatType, MarketStat>();

        [Space(10)]
        // How hard the demand drops initially in response to supply
        [SerializeField] private float _supplySaturation;
        [SerializeField] private float _demandCalculationInterval = 1f;

        [Space(20)]
        [Header("Decay")]
        [SerializeField] private float _baseDecay = 1f;
        [SerializeField] private float _decayTime;
        [SerializeField] private int _decayThreshold; // When supply is above this threshold, timer starts running to drop demand

        [Space(20)]
        [Header("Scarcity")]
        [SerializeField] private float _baseScarcity = 1f;
        [SerializeField] private float _scarcityCoefficient = 0.5f; // Maximum supply threshold at which demand starts to increase significantly
        [SerializeField] private float _midpoint = 5;

        [Space(20)]
        [SerializeField] private float _supplyTime = 10f;
        [SerializeField] private float _priceMultiplier = 10f;

        private float _wealth = 1f;

        private MarketView _marketView;

        private void Awake()
        {
            _marketStats.Add(StatType.Wings, new MarketStat());
            _marketStats.Add(StatType.Horns, new MarketStat());
            _marketStats.Add(StatType.Face, new MarketStat());
            _marketStats.Add(StatType.Armor, new MarketStat());
            _marketStats.Add(StatType.Body, new MarketStat());

            if (_marketView == null)
            {
                _marketView = FindObjectOfType<MarketView>();
                if (_marketView == null)
                {
                    _marketView.toggleMarketStatsLayoutGroup(true);
                    foreach (KeyValuePair<StatType, MarketStat> marketStatKvp in _marketStats)
                    {
                        _marketView.Init(marketStatKvp.Key, marketStatKvp.Value.Price, marketStatKvp.Value.Supply);
                    }
                    _marketView.toggleMarketStatsLayoutGroup(false);
                }
            }

            StartCoroutine(CalculateDemand());
        }

        private void Update()
        {
            // TODO: remove test code
            if (Input.GetKeyDown(KeyCode.F))
            {
                DemonStatsInt stats = new DemonStatsInt(1, 1, 1, 1, 1);
                SupplyDemon(stats);
            }

            foreach (KeyValuePair<StatType, MarketStat> stat in _marketStats)
            {
                stat.Value.UpdateDecay(_baseDecay, _decayTime, _decayThreshold);
            }
        }

        public IEnumerator CalculateDemand()
        {
            while (true)
            {
                foreach (KeyValuePair<StatType, MarketStat> stat in _marketStats)
                {
                    stat.Value.CalculateScarcity(_baseScarcity, _scarcityCoefficient, _midpoint);
                    stat.Value.CalculateDemand(_supplySaturation, _wealth);
                    stat.Value.CalculatePrice(_priceMultiplier);

                    if (_marketView != null)
                    {
                        _marketView.UpdateMarketStat(stat.Key, stat.Value.Price, stat.Value.Supply);
                    }
                   
                    //Debug.Log("Price: " + stat.Value.Price);
                }

                yield return new WaitForSeconds(_demandCalculationInterval);
            }
        }

        public float CalculateDemonPrice(DemonStatsInt demon)
        {
            float price = 0f;

            price += _marketStats[StatType.Wings].Price * demon.Wings;
            price += _marketStats[StatType.Horns].Price * demon.Horn;
            price += _marketStats[StatType.Face].Price * demon.Face;
            price += _marketStats[StatType.Body].Price * demon.Body;
            price += _marketStats[StatType.Armor].Price * demon.Armor;

            return price;
        }

        public void SupplyDemon(DemonStatsInt demon)
        {
            _marketStats[StatType.Wings].Supply += demon.Wings;
            _marketStats[StatType.Horns].Supply += demon.Horn;
            _marketStats[StatType.Face].Supply += demon.Face;
            _marketStats[StatType.Body].Supply += demon.Body;
            _marketStats[StatType.Armor].Supply += demon.Armor;

            StartCoroutine(RemoveFromSupply(demon));
        }

        public void BoostDemand(DemonStatsFloat demon, float time)
        {
            _marketStats[StatType.Wings].Demand += demon.Wings;
            _marketStats[StatType.Horns].Demand += demon.Horn;
            _marketStats[StatType.Face].Demand += demon.Face;
            _marketStats[StatType.Body].Demand += demon.Body;
            _marketStats[StatType.Armor].Demand += demon.Armor;

            StartCoroutine(RemoveFromDemand(demon, time));
        }

        private IEnumerator RemoveFromDemand(DemonStatsFloat demon, float time)
        {
            yield return new WaitForSeconds(time);

            _marketStats[StatType.Wings].Demand -= demon.Wings;
            _marketStats[StatType.Horns].Demand -= demon.Horn;
            _marketStats[StatType.Face].Demand -= demon.Face;
            _marketStats[StatType.Body].Demand -= demon.Body;
            _marketStats[StatType.Armor].Demand -= demon.Armor;
        }

        private IEnumerator RemoveFromSupply(DemonStatsInt demon)
        {
            yield return new WaitForSeconds(_supplyTime);

            _marketStats[StatType.Wings].Supply -= demon.Wings;
            _marketStats[StatType.Horns].Supply -= demon.Horn;
            _marketStats[StatType.Face].Supply -= demon.Face;
            _marketStats[StatType.Body].Supply -= demon.Body;
            _marketStats[StatType.Armor].Supply -= demon.Armor;
        }

        public void AddModifier(StatType stat, float modifier, float time)
        {
            _marketStats[stat].DemandEventModifier *= modifier;
            StartCoroutine(RemoveModifier(stat, modifier, time));
        }

        private IEnumerator RemoveModifier(StatType stat, float modifier, float time)
        {
            yield return new WaitForSeconds(time);
            _marketStats[stat].DemandEventModifier *= 1f / modifier;
        }

        [Serializable]
        private class MarketStat
        {
            public float DemandEventModifier = 1f;
            private float _decay = 1f;
            private float _decayTimePassed = 0f;
            [SerializeField]
            private float _scarcity = 1f;

            public int Supply = 0;
            public float Demand = 10f;

            [SerializeField] private float _price;
            public float Price => _price;

            public void CalculateDemand(float supplySaturation, float wealth)
            {
                // Debug.Log("wealth: " + wealth + "demandeventmod" + DemandEventModifier + "decay: " + _decay + "scarcity: " + _scarcity + "random: " + Random.Range(0.8f, 1.2f) + "supplysaturation: " + supplySaturation );
                Demand = wealth * DemandEventModifier * _decay * Mathf.Max(_scarcity, 1f) /* * Random.Range(0.8f, 1.2f)*/;

            }

            public void UpdateDecay(float baseDecay, float decayTime, int decayThreshold)
            {
                if (Supply > decayThreshold)
                {

                    _decay = Mathf.Max(baseDecay * (1f - _decayTimePassed / decayTime), 0.0001f);
                }
                else
                {
                    _decayTimePassed = 0f;
                    _decay = 1f;
                }
            }

            public void CalculateScarcity(float baseScarcity, float scarcityCoefficient, float midpoint)
            {
                _scarcity = baseScarcity * (1f / (1f + Mathf.Exp(scarcityCoefficient * ((float)Supply - midpoint))));
            }

            public void CalculatePrice(float priceMultiplier)
            {
                _price = Demand * priceMultiplier;
            }
        }
    }

    public enum StatType
    {
        Wings,
        Horns,
        Face,
        Armor,
        Body
    }
}

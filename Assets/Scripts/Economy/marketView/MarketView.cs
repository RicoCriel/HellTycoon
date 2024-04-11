using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace Economy
{
    public class MarketView : MonoBehaviour
    {
        [Header("marketStats")]
        [SerializeField]
        private MarketStatView _marketStatViewPrefab;
        [SerializeField]
        private VerticalLayoutGroup _marketStatsLayoutGroup;
        [SerializeField]
        Transform _marketStatsParent;

        //private dictionaries
        private Dictionary<StatType, MarketStatView> _marketStatViews = new Dictionary<StatType, MarketStatView>();


        public void Init(StatType stat, float partPriceText, float partAmountText)
        {
            MarketStatView marketStatView = Instantiate(_marketStatViewPrefab, _marketStatsParent);
            _marketStatViews.Add(stat, marketStatView);

            marketStatView.Init(stat.ToString(), partPriceText, partAmountText);
        }

        public void UpdateMarketStat(StatType stat, float price, float amount)
        {
            if (_marketStatViews.TryGetValue(stat, out MarketStatView marketStatView))
            {
                marketStatView.UpdateText(price, amount);
            }
        }

        public void toggleMarketStatsLayoutGroup(bool enabled)
        {
            _marketStatsLayoutGroup.enabled = enabled;
        }



    }
}

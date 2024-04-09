using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy
{


    public class YearOverviewUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _year;
        [SerializeField] private TextMeshProUGUI _sales;
        [SerializeField] private TextMeshProUGUI _investments;
        [SerializeField] private TextMeshProUGUI _upkeep;
        [SerializeField] private TextMeshProUGUI _bonus;
        [SerializeField] private TextMeshProUGUI _total;

        public void SetAll(YearData year)
        {
            _year.text = year.YearNumber.ToString();
            _sales.text = year.Income.ToString(CultureInfo.CurrentCulture);
            _investments.text = year.Investments.ToString(CultureInfo.CurrentCulture);
            _upkeep.text = year.Upkeep.ToString(CultureInfo.CurrentCulture);
            _bonus.text = year.Bonus.ToString(CultureInfo.CurrentCulture);
            _total.text = year.Total.ToString(CultureInfo.CurrentCulture);
        }

        public void SetSales(ref YearData year)
        {
            _sales.text = year.Income.ToString(CultureInfo.CurrentCulture);
            _total.text = year.Total.ToString(CultureInfo.CurrentCulture);
        }

        public void SetInvestments(ref YearData year)
        {
            _investments.text = year.Investments.ToString(CultureInfo.CurrentCulture);
            _total.text = year.Total.ToString(CultureInfo.CurrentCulture);
        }

        public void SetUpkeep(ref YearData year)
        {
            _upkeep.text = year.Upkeep.ToString(CultureInfo.CurrentCulture);
            _total.text = year.Total.ToString(CultureInfo.CurrentCulture);
        }

        public void SetBonus(ref YearData year)
        {
            _bonus.text = year.Bonus.ToString(CultureInfo.CurrentCulture);
            _total.text = year.Total.ToString(CultureInfo.CurrentCulture);
        }
    }
}
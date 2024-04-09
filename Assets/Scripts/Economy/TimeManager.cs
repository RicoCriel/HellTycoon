using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Economy
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float _yearDuration;
        [SerializeField] private Queue<YearData> _years;
        [SerializeField] private List<YearOverviewUI> _yearUI;
        [SerializeField] private YearHUD _yearHUD;

        private YearData _currentYear;

        private int _maxYearsStored = 3;

        private void Awake()
        {
            _maxYearsStored = _yearUI.Count;

            _years = new Queue<YearData>();

            _currentYear = new YearData();
            _years.Enqueue(_currentYear);

            if (_yearHUD)
            {
                _yearHUD.SetYear(_currentYear.YearNumber);
            }


            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(_yearDuration);

                var newYear = new YearData();
                if (_currentYear != null)
                {
                    newYear.YearNumber = _currentYear.YearNumber + 1;
                }

                _years.Enqueue(newYear);

                _currentYear = newYear;

                if (_years.Count > _maxYearsStored)
                {
                    _years.Dequeue();
                }

                for (int i = 0; i != _years.Count; ++i)
                {
                    _yearUI[i].SetAll(_years.ElementAt(i));
                }

                if (_yearHUD)
                {
                    _yearHUD.SetYear(_currentYear.YearNumber);
                }
            }
        }

        public void AddTransaction(float amount, TransactionType type)
        {
            _currentYear.AddTransaction(amount, type);

            var idx = _years.Count - 1;

            switch (type)
            {
                case TransactionType.Sale:
                    _yearUI[idx].SetSales(ref _currentYear);
                    break;
                case TransactionType.Investment:
                    _yearUI[idx].SetInvestments(ref _currentYear);
                    break;
                case TransactionType.Bonus:
                    _yearUI[idx].SetBonus(ref _currentYear);
                    break;
                case TransactionType.Upkeep:
                    _yearUI[idx].SetUpkeep(ref _currentYear);
                    break;
            }
        }
    }
}



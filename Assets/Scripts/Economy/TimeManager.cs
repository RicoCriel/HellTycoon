using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economy
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float _yearDuration;
        [SerializeField] private Queue<YearData> _years;
        [SerializeField] private int _maxYearsStored = 3;

        private YearData _currentYear;
        public YearData CurrentYear => _currentYear;

        private void Awake()
        {
            _years = new Queue<YearData>();

            _currentYear = new YearData();
            _years.Enqueue(_currentYear);

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

                Debug.Log(_currentYear.YearNumber);
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economy
{
    public class YearData
    {
        public int YearNumber = 1;

        private float _income = 0f;
        public float Income => _income;

        private float _investments = 0f;
        public float Investments => _investments;

        private float _upkeep = 0f;
        public float Upkeep => _upkeep;

        private float _bonus = 0f;
        public float Bonus => _bonus;

        public float total
        {
            get
            {
                return _income + _upkeep + _investments + _bonus;
            }
        }

        public void AddTransaction(float amount, TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Sale:
                    _income += amount;
                    break;
                case TransactionType.Investment:
                    _investments -= amount;
                    break;
                case TransactionType.Bonus:
                    _bonus += amount;
                    break;
                case TransactionType.Upkeep:
                    _upkeep -= amount;
                    break;
            }
        }
    }
}

using Buildings;
using System.Collections;
using System.Collections.Generic;
using Tycoons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Economy
{
    // Should only be referenced by economymanager and moneyUI
    public class SoulManager : MonoBehaviour
    {
        [SerializeField] private float _startMoney = 200f;
        [SerializeField] private bool _logMoney = false;
        [SerializeField] private bool _godMode = false;
        [SerializeField] private float _deathTimer = 300f;

        [SerializeField] private GameObject _deathTimerDisplay;
        
        private float _money = 0f;
        public float Money => _money;

        private float _deathTimerWeight = 0.05f;
        private float _deathTimerPassed;
        private bool _inDebt;

        private bool _isAIAgent = false;

        public UnityAction OnLost;

        public void Init(TycoonData tycoonData)
        {
            _startMoney = tycoonData.StartMoney;
            _isAIAgent = true;
        }
        void Start()
        {
            _money = _startMoney;
        }

        public void AddMoney(float amount)
        {
            _money += amount;
            if (_logMoney)
            {
                Debug.Log("Money: " + _money);
            }
        }

        public void SubtractMoney(float amount)
        {
            if (_godMode) return;

            _money -= amount;
            if (_logMoney)
            {
                Debug.Log("Money: " + _money);
            }
        }

        public bool IsInDebt()
        {
            return _inDebt;
        }

        private void Update()
        {
            //if (!IsAIAgent)
            //{
            if (_inDebt && _money < 0f)
            {
                _inDebt = true;
                _deathTimerPassed += Time.deltaTime;
            }
            else if (_inDebt)
            {
                _inDebt = false;
                _deathTimer = 0;
                _deathTimerPassed = 0;
            }
            else if (_money < 0f)
            {
                _inDebt = true;
            }

            if (_deathTimerPassed >= _deathTimer - Mathf.Abs(_money * _deathTimerWeight) && _inDebt && !_godMode)
            {
                if (_isAIAgent)
                {
                    OnLost?.Invoke();
                    return;
                }
                SceneManager.LoadScene("Main Menu");
                Debug.Log("Lost game!");
            }
            if (_deathTimerPassed != 0)
            {
                _deathTimerDisplay.SetActive(true);
                _deathTimerDisplay.GetComponent<TMPro.TMP_Text>().text = "Bankrupt!! (In: " + ((int)(_deathTimer - _deathTimerPassed)).ToString() + " sec)";
            }
            else 
            { 
                _deathTimerDisplay.SetActive(false);

            }


            //}
        }
    }
}

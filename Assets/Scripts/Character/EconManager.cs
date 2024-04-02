using Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EconManager : MonoBehaviour
{
   


    [SerializeField] private int _startMoney = 200;
    [SerializeField] private bool _logMoney = false;
    [SerializeField] private bool _godMode = false;

    private int _money = 0;

    public int HornLevelValue = 100;
    public int BodyLevelValue = 150;
    public int FaceLevelValue = 200;
    public int ArmorLevelValue = 250;
    public int WingLevelValue = 300;
    [SerializeField] private float _deathTimer = 300f;
    private float _deathTimerWeight = 0.05f;
    private float _deathTimerPassed;
    private bool _inDebt;

    void Start()
    {
        _money = _startMoney;
    }
    public int GetMoney()
    {
        return _money;
    }
    public void AddMoney(int amount)
    {
        _money += amount;
        if (_logMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }
    
    public void SubtractMoney(int amount)
    {
        if (_godMode) return;

        _money -= amount;
        if (_logMoney)
        {
            Debug.Log("Money: " + _money);
        }
    }


    private void Update()
    {
        if (_money < 0)
        {
            _inDebt = true;
            _deathTimerPassed += Time.deltaTime;
        }
        else
        {
            _inDebt = false;
            _deathTimer = 0;
            _deathTimerPassed = 0;
        }
        if (_deathTimerPassed >= _deathTimer - Mathf.Abs(_money * _deathTimerWeight) && _inDebt && !_godMode)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }


}

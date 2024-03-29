using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFear : MonoBehaviour
{
    private float _fearLevel = 0;
    public float FearLevel => _fearLevel;

    private int _layer;
    public int Layer => _layer;
    [SerializeField] private int _layertHightDiff = 100;

    public float DecayRate;



    private void Start()
    {
        SetLayer();
    }

    
    public float GetFearLevel()
    {
        return _fearLevel;
    }
  

    public void IncreaseFear(float amount)
    {
        _fearLevel += amount;
    }

    public void DecreaseFear(float amount)
    {
        _fearLevel -= amount;
    }

    public void SetLayer()
    {
        _layer = (int)(transform.position.y) / _layertHightDiff;
    }


}

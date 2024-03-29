using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonFear : MonoBehaviour
{
    private int _fearLevel = 0;
    public int FearLevel => _fearLevel;

    private int _layer;
    public int Layer => _layer;
    [SerializeField] private int _layertHightDiff = 100;

    public int DecayRate;



    private void Start()
    {
        SetLayer();
    }

    
    public int GetFearLevel()
    {
        return _fearLevel;
    }
  

    public void IncreaseFear(int amount)
    {
        _fearLevel += amount;
    }

    public void DecreaseFear(int amount)
    {
        _fearLevel -= amount;
    }

    public void SetLayer()
    {
        _layer = (int)(transform.position.y) / _layertHightDiff;
    }


}
